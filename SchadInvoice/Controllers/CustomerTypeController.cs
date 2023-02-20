using AutoMapper;
using BusinessLogic.Entity;
using BusinessLogic.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using SchadInvoice.Models.Dto;
using SchadInvoice.Models.Request;
using System.Data.Entity;

namespace SchadInvoice.Controllers
{
    [Route("api/[controller]")]
    public class CustomerTypeController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerTypeController> _logger;

        public CustomerTypeController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomerTypeController> logger)
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
            return View();
        }

        [HttpGet("edit")]
        public IActionResult Edit(int Id)
        {
            CustomerType entity = FindDataById(Id);
            return View(new CustomerTypeRequest()
            {
                Id = entity.Id,
                Description = entity.Description,
            });
        }
        #endregion

        [HttpGet("Index")]
        public async Task<IActionResult> Index(string filter, int pageIndex = 1, string sortExpression = "Description")
        {
            int pageSize = 10;
            dynamic resultData = null;
            TempData["success"] = true;
            try
            {
                var backendData = this._unitOfWork.CustomerTypeRepository
                .GetIQueryableAll()
                .Select(s => new CustomerTypeDto()
                {
                    Id = s.Id,
                    Description = s.Description
                })
                .OrderBy(on => on.Description)
                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    backendData = backendData.Where(w => w.Description.Contains(filter));
                }

                if (backendData != null)
                {
                    resultData = await PagingList<CustomerTypeDto>.CreateAsync(
                               backendData, pageSize, pageIndex, sortExpression, "Description");

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
                this._unitOfWork.CustomerTypeRepository.Remove(FindDataById(Id));
                this._unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de añadir";
                this._unitOfWork.RejectChanges();
                this._logger.LogError(ex.ToString());
            }
            return RedirectToAction("index", "CustomerType");
        }

        [HttpPost("AddCustomerType")]
        public IActionResult AddCustomerType(CustomerTypeRequest request)
        {
            string backendData = "Datos Guardado exitosamente.";
            try
            {
                if (!ModelState.IsValid)
                {
                    backendData = "La información suministrada no es valida";
                }
                else
                {

                    _unitOfWork.CustomerTypeRepository.Add(new BusinessLogic.Entity.CustomerType()
                    {
                        Description = request.Description
                    });
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                backendData = "Estimado cliente hubo incoveniente en el proceso de añadir ";
                this._logger.LogError(ex.ToString());
            }
            finally
            {
                TempData["message"] = backendData;
            }
            return RedirectToAction("index", "CustomerType");
        }

        [HttpPost("EditCustomerType")]
        public IActionResult EditCustomerType(CustomerTypeRequest request)
        {
            TempData["success"] = true;
            TempData["message"] = "Datos Modificado exitosamente.";
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada no es valida";
                }
                else
                {
                    _unitOfWork.CustomerTypeRepository.Update(new BusinessLogic.Entity.CustomerType()
                    {
                        Id = request.Id,
                        Description = request.Description
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
            return RedirectToAction("index", "CustomerType");
        }

        #region 
        private CustomerType FindDataById(int Id)
        {
            CustomerType entityData = null;
            try
            {
                entityData = this._unitOfWork.CustomerTypeRepository.GetAll()
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
