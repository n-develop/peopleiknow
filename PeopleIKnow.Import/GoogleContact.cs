using CsvHelper.Configuration.Attributes;

namespace PeopleIKnow.Import
{
    public class GoogleContact
    {
        [Name("Name")] public string Name { get; set; }
        [Name("Given Name")] public string GivenName { get; set; }
        [Name("Additional Name")] public string AdditionalName { get; set; }
        [Name("Family Name")] public string FamilyName { get; set; }
        [Name("Yomi Name")] public string YomiName { get; set; }
        [Name("Given Name Yomi")] public string GivenNameYomi { get; set; }
        [Name("Additional Name Yomi")] public string AdditionalNameYomi { get; set; }
        [Name("Family Name Yomi")] public string FamilyNameYomi { get; set; }
        [Name("Name Prefix")] public string NamePrefix { get; set; }
        [Name("Name Suffix")] public string NameSuffix { get; set; }
        [Name("Initials")] public string Initials { get; set; }
        [Name("Nickname")] public string Nickname { get; set; }
        [Name("Short Name")] public string ShortName { get; set; }
        [Name("Maiden Name")] public string MaidenName { get; set; }
        [Name("Birthday")] public string Birthday { get; set; }
        [Name("Gender")] public string Gender { get; set; }
        [Name("Location")] public string Location { get; set; }
        [Name("Billing Information")] public string BillingInformation { get; set; }
        [Name("Directory Server")] public string DirectoryServer { get; set; }
        [Name("Mileage")] public string Mileage { get; set; }
        [Name("Occupation")] public string Occupation { get; set; }
        [Name("Hobby")] public string Hobby { get; set; }
        [Name("Sensitivity")] public string Sensitivity { get; set; }
        [Name("Priority")] public string Priority { get; set; }
        [Name("Subject")] public string Subject { get; set; }
        [Name("Notes")] public string Notes { get; set; }
        [Name("Language")] public string Language { get; set; }
        [Name("Photo")] public string Photo { get; set; }
        [Name("Group Membership")] public string GroupMembership { get; set; }
        [Name("E-mail 1 - Type")] public string Email1Type { get; set; }
        [Name("E-mail 1 - Value")] public string Email1Value { get; set; }
        [Name("E-mail 2 - Type")] public string Email2Type { get; set; }
        [Name("E-mail 2 - Value")] public string Email2Value { get; set; }
        [Name("E-mail 3 - Type")] public string Email3Type { get; set; }
        [Name("E-mail 3 - Value")] public string Email3Value { get; set; }
        [Name("Phone 1 - Type")] public string Phone1Type { get; set; }
        [Name("Phone 1 - Value")] public string Phone1Value { get; set; }
        [Name("Phone 2 - Type")] public string Phone2Type { get; set; }
        [Name("Phone 2 - Value")] public string Phone2Value { get; set; }
        [Name("Phone 3 - Type")] public string Phone3Type { get; set; }
        [Name("Phone 3 - Value")] public string Phone3Value { get; set; }
        [Name("Address 1 - Type")] public string Address1Type { get; set; }
        [Name("Address 1 - Formatted")] public string Address1Formatted { get; set; }
        [Name("Address 1 - Street")] public string Address1Street { get; set; }
        [Name("Address 1 - City")] public string Address1City { get; set; }
        [Name("Address 1 - PO Box")] public string Address1POBox { get; set; }
        [Name("Address 1 - Region")] public string Address1Region { get; set; }
        [Name("Address 1 - Postal Code")] public string Address1PostalCode { get; set; }
        [Name("Address 1 - Country")] public string Address1Country { get; set; }
        [Name("Address 1 - Extended Address")] public string Address1ExtendedAddress { get; set; }
        [Name("Organization 1 - Type")] public string Organization1Type { get; set; }
        [Name("Organization 1 - Name")] public string Organization1Name { get; set; }
        [Name("Organization 1 - Yomi Name")] public string Organization1YomiName { get; set; }
        [Name("Organization 1 - Title")] public string Organization1Title { get; set; }
        [Name("Organization 1 - Department")] public string Organization1Department { get; set; }
        [Name("Organization 1 - Symbol")] public string Organization1Symbol { get; set; }
        [Name("Organization 1 - Location")] public string Organization1Location { get; set; }

        [Name("Organization 1 - Job Description")]
        public string Organization1JobDescription { get; set; }

        [Name("Relation 1 - Type")] public string Relation1Type { get; set; }
        [Name("Relation 1 - Value")] public string Relation1Value { get; set; }
        [Name("Relation 2 - Type")] public string Relation2Type { get; set; }
        [Name("Relation 2 - Value")] public string Relation2Value { get; set; }
        [Name("Relation 3 - Type")] public string Relation3Type { get; set; }
        [Name("Relation 3 - Value")] public string Relation3Value { get; set; }
        [Name("Relation 4 - Type")] public string Relation4Type { get; set; }
        [Name("Relation 4 - Value")] public string Relation4Value { get; set; }
        [Name("Website 1 - Type")] public string Website1Type { get; set; }
        [Name("Website 1 - Value")] public string Website1Value { get; set; }
        [Name("Event 1 - Type")] public string Event1Type { get; set; }
        [Name("Event 1 - Value")] public string Event1Value { get; set; }
    }
}