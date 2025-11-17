public class BankAccount
{
    private decimal _balance;
    public event EventHandler<AccountEventArgs> BalanceChanged;

    public decimal Balance
    {
        get => _balance;
        private set
        {
            if(_balance != value)
            {
                decimal oldBalance = _balance;
                _balance = value;
                decimal changeAmount = value - oldBalance;

                //panggil event setelah logic selesai
                BalanceChanged?.Invoke(this, new AccountEventArgs(oldBalance, value, changeAmount));
                // jadi passingnya pakai new ya buat custom event data class terus passing variablenya ke property di costructor kalau perlu
            }
        }
    }
    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            Balance += amount;
            //memanfaatkan property setter, jadi kek manggil setter gitu mungkin
        }
    }
    public void Withdraw(decimal amount)
    {
        if (amount > 0 && amount <= Balance)
        {
            Balance -= amount;// manggil setter yg ada di property
        }
    }

    // protected virtual void OnBalanceChange(AccountEventArgs e)
    // {
    //     BalanceChanged?.Invoke(this, e);
    // }
    

    // ini kalau mau invokenya dijadiiin method, kalau pakai setter diatas langsung aja gk usah pakai method ini

}
public class AccountEventArgs : EventArgs
{
    //1. AccountEventArgs - Gunakan Properties, bukan Fields
    public decimal OldBalance;
    public decimal NewBalance;
    public decimal ChangeAmount;

    // habis udah definisi in propertynya
    //kemudian definisiin constructornya

    public AccountEventArgs(decimal oldBalance, decimal newBalance, decimal changeAmount)
    {
        OldBalance = oldBalance;
        NewBalance = newBalance;
        ChangeAmount = changeAmount;
    }
}

public class Display
{
    public void LiatSaldo(object sender, AccountEventArgs e)
    {
        System.Console.WriteLine($"Saldo : {e.OldBalance} dan menjadi =>  {e.NewBalance}");
    }
}
/*Berarti kalau butuh custom data event langkahnya:
1. bikin class dengan properti yg diinginkan
2. bikin constructor classnya buat assign property dah*/

public class Program
{
    public static void Main()
    {
        BankAccount bank = new BankAccount();
        Display display = new Display();
        bank.BalanceChanged += display.LiatSaldo;
        bank.Deposit(56.0M);
        bank.Withdraw(20.0M);
    

    }
}