namespace MessageBroker
{
    [AttrModelInfo("Thông tin thân nhân", _API_CONST.CONTACT_PEOPLE)]
    public class oContactPeople
    {
        public int ContactId { set; get; }
        public string FullName { set; get; }
        public int Phone { set; get; }
        public string Address { get; set; }
    }
}
