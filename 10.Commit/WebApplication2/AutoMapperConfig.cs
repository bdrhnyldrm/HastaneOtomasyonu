using AutoMapper;
using WebApplication2.Entities;
using WebApplication2.Models;

namespace WebApplication2
{
	public class AutoMapperConfig : Profile
	{
        public AutoMapperConfig()
        {
            CreateMap<User, UserModel>().ReverseMap(); // User clasını usermodel clasına çevirmeyi öğren.
            CreateMap<User, CreateUserModel>().ReverseMap();
			CreateMap<User, EditUserModel>().ReverseMap();

		}
	}
}
