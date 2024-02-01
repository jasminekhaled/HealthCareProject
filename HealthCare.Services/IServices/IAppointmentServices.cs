using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.IServices
{
    public interface IAppointmentServices
    {
        Task<GeneralResponse<List<ListAppointmentDto>>> ListOfAppointmentOfClinic(int clinicId);
        Task<GeneralResponse<List<ListAppointmentDto>>> ListOfAppointmentOfLab(int labId);
        Task<GeneralResponse<List<ListAppointmentDto>>> ListOfAppointmentOfXray(int xrayId);
        Task<GeneralResponse<List<ReservationResponseDto>>> ListOfReservationsOfDoctor(int hospitalId, int doctorId);
        Task<GeneralResponse<List<PatientReservationDto>>> ListOfReservationsOfPatient(int patientId);// donot forget to delete a reservation after a 24 hours
    //  سؤال اعمله ولا لا // Task<GeneralResponse<List<ReservationResponseDto>>> ListOfReservationsOfAnAppointmentForSpecificDay(int hospitalId, int doctorId, DateOnly date);// donot forget to delete a reservation after a 24 hours
        Task<GeneralResponse<AppointmentResponseDto>> AddClinicAppointment(int clinicId, [FromForm]AddAppointmentRequestDto dto);
        Task<GeneralResponse<ListAppointmentDto>> AddClinicAppointmentDatesToAnAppointment(int clinicAppointmentId, AddAppointmentDateRequestDto dto);
        Task<GeneralResponse<string>> DeleteClinicAppointmentDateOfAnAppointment(int clinicAppointmentDateId);
        Task<GeneralResponse<AppointmentResponseDto>> AddXrayAppointment(int xrayId, AddAppointmentRequestDto dto);
        Task<GeneralResponse<ListAppointmentDto>> AddXrayAppointmentDatesToAnAppointment(int xrayAppointmentId, AddAppointmentDateRequestDto dto);
        Task<GeneralResponse<string>> DeleteXrayAppointmentDateOfAnAppointment(int xrayAppointmentDateId);
        Task<GeneralResponse<AppointmentResponseDto>> AddLabAppointment(int labId, AddAppointmentRequestDto dto);
        Task<GeneralResponse<ListAppointmentDto>> AddLabAppointmentDatesToAnAppointment(int labAppointmentId, AddAppointmentDateRequestDto dto);
        Task<GeneralResponse<string>> DeleteLabAppointmentDateOfAnAppointment(int labAppointmentDateId);
        Task<GeneralResponse<string>> DeleteClinicAppointment(int clinicAppointmentId);//notification
        Task<GeneralResponse<string>> DeleteXrayAppointment(int xrayAppointmentId);
        Task<GeneralResponse<string>> DeleteLabAppointment(int labAppointmentId);
        Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfClinic(int patientId, int clinicAppointmentDateId, string date);
        Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfXray(int patientId, int xrayAppointmentDateId, string date);
        Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfLab(int patientId, int labAppointmentDateId, string date);
        Task<GeneralResponse<string>> CancelClinicReservation(int patientId, int clinicReservationId);
        Task<GeneralResponse<string>> CancelXrayReservation(int patientId, int xrayReservationId);
        Task<GeneralResponse<string>> CancelLabReservation(int patientId, int labReservationId);
        //   Task<GeneralResponse<string>> NumOfReservationsOfAnAppointmentForSpecificDay(int AppointmentId, DateOnly date);
        //  Task<GeneralResponse<string>> NumOfAllReservationsOfAnAppointment();
        Task<GeneralResponse<string>> DoneReservation(int reservationId);



    }
}
