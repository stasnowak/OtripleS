﻿using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;
using OtripleS.Web.Api.Services.Attendances;
using RESTFulSense.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : RESTFulController
    {
        private readonly IAttendanceService attendanceService;

        public AttendanceController(IAttendanceService attendanceService) =>
            this.attendanceService = attendanceService;

        [HttpGet]
        public ActionResult<IQueryable<Attendance>> GetAllAttendances()
        {
            try
            {
                IQueryable storageAttendance =
                    this.attendanceService.RetrieveAllAttendances();

                return Ok();
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return Problem(attendanceDependencyException.Message);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return Problem(attendanceServiceException.Message);
            }
        }

        [HttpGet("{attendanceId}")]
        public async ValueTask<ActionResult<Attendance>> GetAttendanceByIdAsync(Guid attendanceId)
        {
            try
            {
                Attendance attendance =
                    await this.attendanceService.RetrieveAttendanceByIdAsync(attendanceId);

                return Ok(attendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                string innerMessage = GetInnerMessage(attendanceValidationException);

                return NotFound(innerMessage);
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                string innerMessage = GetInnerMessage(attendanceValidationException);

                return BadRequest(innerMessage);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return Problem(attendanceDependencyException.Message);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return Problem(attendanceServiceException.Message);
            }
        }

        [HttpDelete("{attendanceId}")]
        public async ValueTask<ActionResult<Attendance>> DeleteAttendanceAsync(Guid attendanceId)
        {
            try
            {
                Attendance storageAttendance =
                    await this.attendanceService.DeleteAttendanceAsync(attendanceId);

                return Ok(storageAttendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                string innerMessage = GetInnerMessage(attendanceValidationException);

                return NotFound(innerMessage);
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                return BadRequest(attendanceValidationException.Message);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
                when (attendanceDependencyException.InnerException is LockedAttendanceException)
            {
                string innerMessage = GetInnerMessage(attendanceDependencyException);

                return Locked(innerMessage);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return Problem(attendanceDependencyException.Message);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return Problem(attendanceServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Attendance>> PutAttendanceAsync(Attendance attendance)
        {
            try
            {
                Attendance registeredAttendance =
                    await this.attendanceService.ModifyAttendanceAsync(attendance);

                return Ok(registeredAttendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                string innerMessage = GetInnerMessage(attendanceValidationException);

                return NotFound(innerMessage);
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                string innerMessage = GetInnerMessage(attendanceValidationException);

                return BadRequest(innerMessage);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
                when (attendanceDependencyException.InnerException is LockedAttendanceException)
            {
                string innerMessage = GetInnerMessage(attendanceDependencyException);

                return Locked(innerMessage);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return Problem(attendanceDependencyException.Message);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return Problem(attendanceServiceException.Message);
            }
        }

        private string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
        
    }
}