using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;
        private readonly UserValidator _userValidator;
        private readonly CreditLimitHandler _creditLimitHandler;

        public UserService(IClientRepository clientRepository, IUserCreditService userCreditService, UserValidator userValidator, CreditLimitHandler creditLimitHandler)
        {
            _clientRepository = clientRepository;
            _userCreditService = userCreditService;
            _userValidator = userValidator;
            _creditLimitHandler = creditLimitHandler;
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!_userValidator.Validate(firstName, lastName, email, dateOfBirth))
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            _creditLimitHandler.Handle(user, client);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
    }
    
    public class UserValidator
    {
        public bool Validate(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            return true;
        }
    }
    public class CreditLimitHandler
    {
        private readonly IUserCreditService _userCreditService;

        public CreditLimitHandler(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }

        public void Handle(User user, Client client)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit = creditLimit * 2;
                user.CreditLimit = creditLimit;
            }
            else
            {
                user.HasCreditLimit = true;
                int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }
        }
    }
    
    public interface IClientRepository
    {
        Client GetById(int clientId);
    }
    
    public interface IUserCreditService
    {
        int GetCreditLimit(string lastName, DateTime dateOfBirth);
    }
}
