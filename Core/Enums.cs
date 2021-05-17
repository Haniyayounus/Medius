namespace Core
{
    public enum UserType
    {
        AppUser = 1,
        SubAdmin = 2,
        SuperAdmin = 3
    }

    public enum FilterType
    {
        Category = 1,
        Technology = 2
    }

    public enum IpType
    {
        Copyright = 1,
        Patent = 2,
        Trademark = 3,
        Design = 4
    }

    public enum IpStatus
    {
        Registered = 0,
        Pending = 1,
        UnderReview = 2,
        Approved = 3
    }
}
