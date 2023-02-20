using AutoMapper;
using BusinessLogic.Entity;
using BusinessLogic.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using SchadInvoice.Models.Dto;
using SchadInvoice.Models.Request;
using System.Collections.Generic;
using System.Data.Entity;

namespace SchadInvoice.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomerController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        #region view
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("add")]
        public IActionResult Add()
        {
            return View(new CustomerRequest()
            {
                CustomerTypes = GetCustomerTypeDatas()
            });
        }

        [HttpGet("edit")]
        public IActionResult Edit(int Id)
        {
            Customer entity = FindDataById(Id);
            List<CustomerTypeDto> customerType = GetCustomerTypeDatas();

            return View(new CustomerRequest()
            {
               Customer = new CustomerDto() 
               {
                   Id = entity.Id,
                   CustName = entity.CustName,
                   Adress = entity.Adress,
                   Status = entity.Status,
                   CustomerTypeId = entity.CustomerTypeId,
                   CustomerTypeDescription = customerType.Where(w=> w.Id == entity.CustomerTypeId).FirstOrDefault().Description
               },
               CustomerTypes = customerType
            });
        }
        #endregion

        [HttpGet("Index")]
        public async Task<IActionResult> Index(string filter, int pageIndex = 1, string sortExpression = "CustName")
        {
            int pageSize = 10;
            dynamic resultData = null;
            TempData["success"] = true;
            try
            {
                var backendData = this._unitOfWork.CustomerRepository
                .GetIQueryableAll()
                .Include( i => i.CustomerType)
                .Select(s => new CustomerDto()
                {
                    Id = s.Id,
                    CustName = s.CustName,
                    Adress = s.Adress,
                    Status = s.Status,
                    CustomerTypeId = s.CustomerType.Id,
                    CustomerTypeDescription = s.CustomerType.Description
                })
                .OrderBy(on => on.CustName)
                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    backendData = backendData.Where(w => w.CustName.Contains(filter));
                }

                if (backendData != null)
                {
                    resultData = await PagingList<CustomerDto>.CreateAsync(
                               backendData, pageSize, pageIndex, sortExpression, "CustName");

                    resultData.RouteValue = new RouteValueDictionary {
                        { "filter", filter}
                    };
                }
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de añadir";
                this._logger.LogError(ex.ToString());
            }

            return View(resultData);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int Id)
        {
            TempData["success"] = true;
            TempData["message"] = "Datos Modificado Exitosamente.";
            try
            {
                this._unitOfWork.CustomerRepository.Remove(FindDataById(Id));
                this._unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de Borrar";
                this._unitOfWork.RejectChanges();
                this._logger.LogError(ex.ToString());
            }
            return RedirectToAction("index", "Customer");
        }

        [HttpPost("AddEntity")]
        public IActionResult AddEntity(CustomerRequest request)
        {
            TempData["success"] = true;
            TempData["message"] = "Datos guardos satifactoriamente";
            try
            {
                if (string.IsNullOrEmpty(request.Customer.CustName))
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada de custName no es valida";
                }
                else if (string.IsNullOrEmpty(request.Customer.Adress))
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada de Adress no es valida";
                }
                else
                {
                    _unitOfWork.CustomerRepository.Add(new BusinessLogic.Entity.Customer()
                    {
                        CustName = request.Customer.CustName,
                        Adress = request.Customer.Adress,
                        Status = request.Customer.Status,
                        CustomerTypeId = request.Customer.CustomerTypeId,
                    });
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de añadir";
                this._logger.LogError(ex.ToString());
            }

            return RedirectToAction("index", "Customer");
        }

        [HttpPost("EditEntity")]
        public IActionResult EditEntity(CustomerRequest request)
        {
            TempData["success"] = true;
            TempData["message"] = "Datos Modificado exitosamente.";
            try
            {
                if (string.IsNullOrEmpty(request.Customer.CustName))
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada de custName no es valida";
                }
                else if (string.IsNullOrEmpty(request.Customer.Adress))
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada de Adress no es valida";
                }
                else
                {
                    _unitOfWork.CustomerRepository.Update(new BusinessLogic.Entity.Customer()
                    {
                        Id = request.Customer.Id,
                        CustName = request.Customer.CustName,
                        Adress = request.Customer.Adress,
                        Status = request.Customer.Status,
                        CustomerTypeId = request.Customer.CustomerTypeId,
                    });
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de Modificación ";
                this._logger.LogError(ex.ToString());
            }
            return RedirectToAction("index", "Customer");
        }

        #region 

        private List<CustomerTypeDto> GetCustomerTypeDatas()
        {
            List<CustomerType> entityDatas = null;
            List<CustomerTypeDto> backendData = null;
            try
            {
                entityDatas = this._unitOfWork.CustomerTypeRepository.GetAll()
                    .ToList();

                if (entityDatas == null)
                {
                    throw new Exception("No se pudo encontrar la información segun el Id");
                }
                backendData = _mapper.Map<List<CustomerTypeDto>>(entityDatas);
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de buscar información";
                this._logger.LogError(ex.ToString());
            }

            return backendData;
        }
        
        private Customer FindDataById(int Id)
        {
            Customer entityData = null;
            try
            {
                entityData = this._unitOfWork.CustomerRepository.GetAll()
                    .Where(w => w.Id == Id)
                    .FirstOrDefault();

                if (entityData == null)
                {
                    throw new Exception("No se pudo encontrar la información segun el Id");
                }
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de buscar información";
                this._logger.LogError(ex.ToString());
            }
            return entityData;
        }

        #endregion
    }
}
