using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using UIM.Core.Common;
using UIM.Core.Helpers;
using UIM.Core.Models.Dtos;
using UIM.Core.Models.Dtos.Department;
using UIM.Core.Models.Entities;
using UIM.Core.ResponseMessages;
using UIM.Core.Services.Interfaces;

namespace UIM.Core.Services
{
    public class DepartmentService : Service, IDepartmentService
    {
        public DepartmentService(IMapper mapper,
            IOptions<SieveOptions> sieveOptions,
            SieveProcessor sieveProcessor,
            IUnitOfWork unitOfWork)
            : base(mapper,
                sieveOptions,
                sieveProcessor,
                unitOfWork)
        {
        }

        public async Task CreateAsync(CreateDepartmentRequest request)
        {
            if (await _unitOfWork.Departments.GetByNameAsync(request.Name) != null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var added = await _unitOfWork.Departments.AddAsync(
                new Department { Name = request.Name });
            if (!added)
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        ErrorResponseMessages.UnexpectedError);
        }

        public async Task EditAsync(int departmentId, UpdateDepartmentRequest request)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
            if (department == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            department.Name = request.Name;
            var edited = await _unitOfWork.Departments.UpdateAsync(department);
            if (!edited)
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        ErrorResponseMessages.UnexpectedError);
        }

        public async Task<TableResponse> FindAsync(SieveModel model)
        {
            if (model?.Page < 0 || model?.PageSize < 1)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var departments = await _unitOfWork.Departments.GetAllAsync();
            var sortedDepartments = _sieveProcessor.Apply(model, departments.AsQueryable());

            var pageSize = model.PageSize ?? _sieveOptions.DefaultPageSize;

            return new(sortedDepartments, sortedDepartments.Count(),
                currentPage: model.Page ?? 1,
                totalPages: (int)Math.Ceiling((float)departments.Count() / pageSize));
        }

        public async Task<DepartmentDetailsResponse> FindByIdAsync(int departmentId)
        {
            var department = _mapper.Map<DepartmentDetailsResponse>(
                await _unitOfWork.Departments.GetByIdAsync(departmentId));
            return department;
        }

        public async Task RemoveAsync(int departmentId)
        {
            var succeeded = await _unitOfWork.Departments.DeleteAsync(departmentId);
            if (!succeeded)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);
        }
    }
}