using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHapi.Model.V251.Message;

//right now this is populated with info for the fake "Christopher Moltisanti" test patient.
//Th plan is to use this "Builder" design pattern for all of the rest of the HL7 messages that the program will eventually have
//The constructor of the Builder class will eventually have info passed into it from the main program as it makes messages for each patient,
//and then will use those input parameters to make the HL7 message and then return it to the main program.

namespace MINERVA
{
    public class Adt01MessageBuilder
    {
        private ADT_A01 _adtMessage;



        public ADT_A01 Build()
        {
            var currentDateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
            _adtMessage = new ADT_A01();

            CreateMshSegment(currentDateTimeString);
            CreateEvnSegment(currentDateTimeString);
            CreatePidSegment();
            CreatePv1Segment(currentDateTimeString);

            return _adtMessage;
        }

        private void CreateMshSegment(string currentDateTimeString)
        {
            var mshSegment = _adtMessage.MSH;
            mshSegment.FieldSeparator.Value = "|";
            mshSegment.EncodingCharacters.Value = "^~\\&";
            mshSegment.SendingApplication.NamespaceID.Value = "RadOnc Minerva";
            mshSegment.SendingFacility.NamespaceID.Value = "Lahey Radiation Oncology";
            mshSegment.ReceivingApplication.NamespaceID.Value = "Epic";
            mshSegment.ReceivingFacility.NamespaceID.Value = "Lahey Hospital";
            mshSegment.DateTimeOfMessage.Time.Value = currentDateTimeString;
            mshSegment.MessageControlID.Value = GetSequenceNumber(currentDateTimeString);
            mshSegment.MessageType.MessageCode.Value = "ADT";
            mshSegment.MessageType.TriggerEvent.Value = "A01";
            mshSegment.VersionID.VersionID.Value = "2.5.1";
            mshSegment.ProcessingID.ProcessingID.Value = "P";
        }

        private void CreateEvnSegment(string currentDateTimeString)
        {
            var evn = _adtMessage.EVN;
            evn.EventTypeCode.Value = "A01";
            evn.RecordedDateTime.Time.Value = currentDateTimeString;
        }

        private void CreatePidSegment()
        {
            var pid = _adtMessage.PID;
            var patientName = pid.GetPatientName(0);
            patientName.FamilyName.Surname.Value = "Moltisanti";
            patientName.GivenName.Value = "Christopher";
            pid.PatientID.IDNumber.Value = "378785433211";
            var patientAddress = pid.GetPatientAddress(0);
            patientAddress.StreetAddress.DwellingNumber.Value = "3";
            patientAddress.StreetAddress.StreetName.Value = "Baldwin Ct";
            patientAddress.City.Value = "Fairfield";
            patientAddress.StateOrProvince.Value = "NJ";
            patientAddress.Country.Value = "USA";
        }

        private void CreatePv1Segment(string currentDateTimeString)
        {
            var pv1 = _adtMessage.PV1;
            pv1.PatientClass.Value = "O"; // to represent an 'Outpatient'
            var assignedPatientLocation = pv1.AssignedPatientLocation;
            assignedPatientLocation.Facility.NamespaceID.Value = "BUR Radiation Oncology";
            assignedPatientLocation.PointOfCare.Value = "Some Point of Care";
            pv1.AdmissionType.Value = "ALERT";
            var referringDoctor = pv1.GetReferringDoctor(0);
            referringDoctor.IDNumber.Value = "99999999";
            referringDoctor.FamilyName.Surname.Value = "Smith";
            referringDoctor.GivenName.Value = "Jack";
            referringDoctor.IdentifierTypeCode.Value = "456789";
            pv1.AdmitDateTime.Time.Value = currentDateTimeString;
        }


        private static string GetSequenceNumber(string currentDateTimeString)
        {
            const string facilityNumberPrefix = "1234"; // some arbitrary prefix for the facility
            return facilityNumberPrefix + currentDateTimeString;
        }




    }
}
