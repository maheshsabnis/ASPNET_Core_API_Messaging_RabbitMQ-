using Core_SenderAPI.Models;
namespace Core_SenderAPI.Logic
{
    public class ProcessOrder
    {
        ItemDb items;
        private double expectedAdvance, totalBillAmount;
        QueuePublisher publisher;
        public ProcessOrder()
        {
            items = new ItemDb();   
        }
        private bool ApproveOrder(Order order)
        {
            bool isOrderApproved = false;
             
                // Get the Unit Price for the Item
                double unitPrice = (from item in items
                                    where item.ItemName == order.OrderedItem
                                    select item).FirstOrDefault().UnitPrice;
            // Calculate the Total Amount
            totalBillAmount = unitPrice * order.Quantity;
                // Calculate the expected Advance against the Order
                 expectedAdvance = totalBillAmount * 0.2;
            // Please check if the advance amount is less than the expected advance

            isOrderApproved = order.AdvanceAmount >= expectedAdvance ? true : false;
           return isOrderApproved;
        }

        public ResponseObject ManageOrder(Order order)
        {
            var response =new ResponseObject();
            Random rnd = new Random();
            if (ApproveOrder(order))
            {
                response.ApproveId = rnd.NextInt64(long.MaxValue);
                response.Message = "Congratulation!!! Your Order is Approved and send for the Procesing.";
                // Publish Message to Queue
                publisher = new QueuePublisher();
                publisher.PublishMessage(order, totalBillAmount, expectedAdvance);
            }
            else
            {
                response.RejectionId  = rnd.NextInt64(long.MaxValue);
                response.Message = $"Sorry!!! Your Order is Rejected because the Total Amount to pay is {totalBillAmount} and the expected advance is gearetr than or equalt to {expectedAdvance}, but the received advance is {order.AdvanceAmount}";
            }
            return response;
        }
    }
}
