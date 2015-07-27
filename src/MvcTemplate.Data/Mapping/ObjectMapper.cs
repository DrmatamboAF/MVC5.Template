using AutoMapper;
using MvcTemplate.Objects;

namespace MvcTemplate.Data.Mapping
{
    public static class ObjectMapper
    {
        public static void MapObjects()
        {
            MapRoles();
            MapAccounts();
        }

        #region Administration

        private static void MapRoles()
        {
            Mapper.CreateMap<Role, RoleView>();
            Mapper.CreateMap<RoleView, Role>();
        }
        private static void MapAccounts()
        {
            Mapper.CreateMap<Account, AccountView>();
            Mapper.CreateMap<Account, AccountEditView>();
            Mapper.CreateMap<Account, ProfileEditView>();

            Mapper.CreateMap<AccountCreateView, Account>();
            Mapper.CreateMap<AccountRegisterView, Account>();
        }

        #endregion
    }
}
