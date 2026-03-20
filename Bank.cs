using System;

namespace SOLID_Fundamentals
{
    
    public abstract class Account
    {
        public decimal Balance { get; protected set; }

        public virtual void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public abstract void Withdraw(decimal amount);

        public virtual decimal CalculateInterest()
        {
            return Balance * 0.01m;
        }
    }

    
    public class SavingsAccount : Account
    {
        public decimal MinimumBalance { get; } = 100m;

        public override void Withdraw(decimal amount)
        {
            if (Balance - amount < MinimumBalance)
            {
                throw new InvalidOperationException("Cannot go below minimum balance");
            }
            Balance -= amount;
        }
    }

    public class CheckingAccount : Account
    {
        public decimal OverdraftLimit { get; } = 500m;

        public override void Withdraw(decimal amount)
        {
            if (Balance - amount < -OverdraftLimit)
            {
                throw new InvalidOperationException("Overdraft limit exceeded");
            }
            Balance -= amount;
        }
    }

    public class FixedDepositAccount : Account
    {
        public DateTime MaturityDate { get; }

        public FixedDepositAccount(DateTime maturityDate)
        {
            MaturityDate = maturityDate;
        }

        public override void Withdraw(decimal amount)
        {
            if (DateTime.Now < MaturityDate)
            {
                throw new InvalidOperationException("Cannot withdraw before maturity date");
            }

            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds");
            }

            Balance -= amount;
        }

        public override decimal CalculateInterest()
        {
            return Balance * 0.05m;
        }
    }

    // ОПА, вот и новый интерфейс для стратегии снятия 
    public interface IWithdrawalStrategy
    {
        void Execute(Account account, decimal amount);
    }

    // Стратегия: стандартное снятие (как в Bank.ProcessWithdrawal) 
    public class StandardWithdrawalStrategy : IWithdrawalStrategy
    {
        public void Execute(Account account, decimal amount)
        {
            try
            {
                account.Withdraw(amount);
                Console.WriteLine($"Successfully withdrew {amount}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Withdrawal failed: {ex.Message}");
            }
        }
    }

  
    public class LoggedWithdrawalStrategy : IWithdrawalStrategy
    {
        private readonly IWithdrawalStrategy _innerStrategy;

        public LoggedWithdrawalStrategy(IWithdrawalStrategy innerStrategy)
        {
            _innerStrategy = innerStrategy;
        }

        public void Execute(Account account, decimal amount)
        {
            Console.WriteLine($"[LOG] Attempting to withdraw {amount} from account.");
            _innerStrategy.Execute(account, amount);
            Console.WriteLine($"[LOG] Withdrawal attempt completed.");
        }
    }

    // Банк теперь принимает стратегию — открыт для расширения 
    public class Bank
    {
        private readonly IWithdrawalStrategy _withdrawalStrategy;

        public Bank(IWithdrawalStrategy withdrawalStrategy)
        {
            _withdrawalStrategy = withdrawalStrategy ?? throw new ArgumentNullException(nameof(withdrawalStrategy));
        }

        // Теперь метод не меняется при добавлении новой логики
        public void ProcessWithdrawal(Account account, decimal amount)
        {
            _withdrawalStrategy.Execute(account, amount);
        }

        public void Transfer(Account from, Account to, decimal amount)
        {
           
            _withdrawalStrategy.Execute(from, amount);
            to.Deposit(amount);
        }
    }
}
