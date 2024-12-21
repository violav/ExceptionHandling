using BusinessLogic.Services;

namespace BusinessLogic.DomainDataError
{
    public static class BusinessError
    { 
        public static class BusinessMatErrors
        {
            public static readonly (string, object) CategoryAlreadyExists = ("ERR004", BusinessResource.CategoryAlreadyExists );
            public static readonly (string, string) CategoryAlreadyExists2 = ( "ERR001", BusinessResource.CategoryAlreadyExists2);

        }
    }
}
