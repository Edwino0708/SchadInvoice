using AutoMapper;
using BusinessLogic.Entity;
using BusinessLogic.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using SchadInvoice.Models.Dto;
using SchadInvoice.Models.Request;

namespace SchadInvoice.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<InvoiceController> logger)
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
            InvoiceRequest modelRequest = new InvoiceRequest()
            {
                Customers = GetCustomerDatas()
            };
            return View(modelRequest);
        }

        [HttpGet("edit")]
        public IActionResult Edit(int Id)
        {
            Invoice invoiceData = FindDataById(Id);
            return View(new InvoiceRequest()
            {
                Customers = GetCustomerDatas(),
                Invoice = _mapper.Map<InvoiceDto>(invoiceData),
                InvoiceDetail = _mapper.Map<InvoiceDetailDto>(FindDataDetailById(invoiceData.Id))
            });
        }

        [HttpGet("detail")]
        public IActionResult Detail(int Id)
        {
            Invoice invoiceData = FindDataById(Id);
            return View(new InvoiceRequest()
            {
                Invoice = _mapper.Map<InvoiceDto>(invoiceData),
                InvoiceDetail = _mapper.Map<InvoiceDetailDto>(FindDataDetailById(invoiceData.Id))
            });
        }

        #endregion

        [HttpGet("Index")]
        public async Task<IActionResult> Index(string filter, int pageIndex = 1, string sortExpression = "CustomerName")
        {
            int pageSize = 10;
            dynamic resultData = null;
            TempData["success"] = true;
            try
            {
                var backendData = this._unitOfWork.InvoiceRepository
                .GetIQueryableAll()
                .Include(x => x.Customer)
                .Select(s => new InvoiceDto()
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    TotalItbis = s.TotalItbis,
                    SubTotal = s.SubTotal,
                    Total = s.Total,
                    CustomerName = s.Customer.CustName
                })
                .OrderBy(on => on.CustomerId)
                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    backendData = backendData.Where(w => w.CustomerName == filter);
                }

                if (backendData != null)
                {
                    resultData = await PagingList<InvoiceDto>.CreateAsync(
                               backendData, pageSize, pageIndex, sortExpression, "CustomerId");

                    resultData.RouteValue = new RouteValueDictionary {
                        { "filter", filter}
                    };
                }
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de lista";
                this._logger.LogError(ex.ToString());
            }

            return View(resultData);
        }

        [HttpPost("AddEntity")]
        public IActionResult AddEntity(InvoiceRequest request)
        {
            TempData["success"] = true;
            TempData["message"] = "Datos guardos satifactoriamente";
            try
            {
                if (request.Invoice.CustomerId == 0)
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada de CustomerId no es valida";
                }
                else if (request.Invoice.Total == 0)
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada de Total no es valida";
                }
                else
                {
                    _unitOfWork.InvoiceRepository.Add(new BusinessLogic.Entity.Invoice()
                    {
                        CustomerId = request.Invoice.CustomerId,
                        TotalItbis = request.Invoice.TotalItbis,
                        SubTotal = request.Invoice.SubTotal,
                        Total = request.Invoice.Total
                    });

                    _unitOfWork.InvoiceDetailRepository.Add(new BusinessLogic.Entity.InvoiceDetail()
                    {
                        CustomerId = request.Invoice.CustomerId,
                        TotalItbis = request.Invoice.TotalItbis,
                        SubTotal = request.Invoice.SubTotal,
                        Total = request.Invoice.Total,
                        Qty = request.InvoiceDetail.Qty,
                        Price = request.InvoiceDetail.Price
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

        [HttpGet("Delete")]
        public IActionResult Delete(int Id)
        {
            TempData["success"] = true;
            TempData["message"] = "Datos Modificado Exitosamente.";
            try
            {
                this._unitOfWork.InvoiceRepository.Remove(FindDataById(Id));
                this._unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de Borrar";
                this._unitOfWork.RejectChanges();
                this._logger.LogError(ex.ToString());
            }
            return RedirectToAction("index", "Invoice");
        }

        [HttpPost("EditEntity")]
        public IActionResult EditEntity(InvoiceRequest request)
        {
            TempData["success"] = true;
            TempData["message"] = "Datos Modificado exitosamente.";
            try
            {
                if (request.Invoice.Id == 0)
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada de Id no es valida";
                }
                else if (request.Invoice.CustomerId == 0)
                {
                    TempData["success"] = false;
                    TempData["message"] = "La información suministrada de CustomerId no es valida";
                }
                else
                {
                    _unitOfWork.InvoiceRepository.Update(new BusinessLogic.Entity.Invoice()
                    {
                        Id = request.Invoice.Id,
                        CustomerId = request.Invoice.CustomerId,
                        TotalItbis = request.Invoice.TotalItbis,
                        SubTotal = request.Invoice.SubTotal,
                        Total = request.Invoice.Total
                    });

                    _unitOfWork.InvoiceDetailRepository.Update(new BusinessLogic.Entity.InvoiceDetail()
                    {
                        Id = request.InvoiceDetail.Id,
                        CustomerId = request.Invoice.CustomerId,
                        TotalItbis = request.Invoice.TotalItbis,
                        SubTotal = request.Invoice.SubTotal,
                        Total = request.Invoice.Total,
                        Qty = request.InvoiceDetail.Qty,
                        Price = request.InvoiceDetail.Price
                    });
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de Modificación";
                this._logger.LogError(ex.ToString());
            }
            return RedirectToAction("index", "Invoice");
        }


        #region 
        private List<CustomerDto> GetCustomerDatas()
        {
            List<Customer> entityDatas = null;
            List<CustomerDto> backendData = null;
            try
            {
                entityDatas = this._unitOfWork.CustomerRepository.GetAll()
                    .ToList();

                if (entityDatas == null)
                {
                    throw new Exception("No se pudo encontrar la información segun el Id");
                }
                backendData = _mapper.Map<List<CustomerDto>>(entityDatas);
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = "Estimado cliente hubo incoveniente en el proceso de buscar información";
                this._logger.LogError(ex.ToString());
            }

            return backendData;
        }

        private Invoice FindDataById(int Id)
        {
            Invoice entityData = null;
            try
            {
                entityData = this._unitOfWork.InvoiceRepository.GetIQueryableAll()
                     .Where(w => w.Id == Id)
                     .Include(x => x.Customer)
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

        private InvoiceDetail FindDataDetailById(int invoiceId)
        {
            InvoiceDetail entityData = null;
            try
            {
                entityData = this._unitOfWork.InvoiceDetailRepository.GetAll()
                    .Where(w => w.Id == invoiceId)
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
