

namespace VendingMachine
{
    public class VendingMachine
    {

        public enum MachineState
        {
            Ready,
            ProductSelected,
            PaymentPending,
            OutOfStock
        }


        private MachineState currentState;

        public VendingMachine()
        {
            currentState = MachineState.Ready;
        }

     
        public void HandleRequest()
        {
        
            if (currentState == MachineState.Ready)
            {
                Console.WriteLine("Ready state: Please select a product.");
            }
            else if (currentState == MachineState.ProductSelected)
            {
                Console.WriteLine("Product selected state: Processing payment.");
            }
            else if (currentState == MachineState.PaymentPending)
            {
                Console.WriteLine("Payment pending state: Dispensing product.");
            }
            else if (currentState == MachineState.OutOfStock)
            {
                Console.WriteLine("Out of stock state: Please select another product.");
            }
        }


        public void ChangeState(MachineState newState)
        {
            currentState = newState;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            VendingMachine vendingMachine = new VendingMachine();

            vendingMachine.ChangeState(VendingMachine.MachineState.Ready);
            vendingMachine.HandleRequest();

            vendingMachine.ChangeState(VendingMachine.MachineState.ProductSelected);
            vendingMachine.HandleRequest();

            vendingMachine.ChangeState(VendingMachine.MachineState.PaymentPending);
            vendingMachine.HandleRequest();

            vendingMachine.ChangeState(VendingMachine.MachineState.OutOfStock);
            vendingMachine.HandleRequest();
        }
    }
}







// public interface IVendingMachineState
// {
//     void HandleRequest();
// }


// public class ReadyState : IVendingMachineState
// {
//     public void HandleRequest()
//     {
//         Console.WriteLine("Ready state: Please select a product.");
//     }
// }

// public class ProductSelectedState : IVendingMachineState
// {
//     public void HandleRequest()
//     {
//         Console.WriteLine("Product selected state: Processing payment.");
//     }
// }

// public class PaymentPendingState : IVendingMachineState
// {
//     public void HandleRequest()
//     {
//         Console.WriteLine("Payment pending state: Dispensing product.");
//     }
// }

// public class OutOfStockState : IVendingMachineState
// {
//     public void HandleRequest()
//     {
//         Console.WriteLine("Out of stock state: Please select another product.");
//     }
// }

// public class VendingMachineContext
// {
//     private IVendingMachineState _state;

//     public void SetState(IVendingMachineState state)
//     {
//         _state = state;
//     }

//     public void Request()
//     {
//         _state.HandleRequest();
//     }
// }


// // class Program
// // {
// //     static void Main(string[] args)
// //     {

// //         VendingMachineContext vendingMachine = new VendingMachineContext();

// //         vendingMachine.SetState(new ReadyState());
// //         vendingMachine.Request();

// //         vendingMachine.SetState(new ProductSelectedState());
// //         vendingMachine.Request();

// //         vendingMachine.SetState(new PaymentPendingState());
// //         vendingMachine.Request();

// //         vendingMachine.SetState(new OutOfStockState());
// //         vendingMachine.Request();

// //     }
//     }