using System;
using System.Collections.Generic;

namespace API.Models
{
    public class Filters
    {
        public Filters(List<int> product_ids, List<int> status_ids, DateTime date_from, DateTime date_to)
        {
            this.product_ids = product_ids;
            this.status_ids = status_ids;
            this.date_from = date_from;
            this.date_to = date_to;
        }

        public List<int> product_ids { get; set; }
        public List<int> status_ids { get; set; }
        public DateTime date_from { get; set; }
        public DateTime date_to { get; set; }

        public List<Ticket> filterTickets(List<Ticket> input)
        {

            // Console.WriteLine("Filtering Products: " + stringFromList(product_ids));
            // Console.WriteLine("Filtering Status: " + stringFromList(status_ids));
            // Console.WriteLine("Filtering dates from " + date_from.ToString() + " to " + date_to.ToString());

            input.RemoveAll(ticketToRemove);

            // foreach (var ticket in input)
            // {
            //     Console.WriteLine("Input now has: " + ticket.title);
            // }

            // Console.WriteLine("Finished filtering************");

            return input;
        }

        private bool ticketToRemove(Ticket ticket)
        {
            bool product_check = (!product_ids.Contains(ticket.product_id));

            bool status_check = (!status_ids.Contains(ticket.status_id));

            bool date_check = ((ticket.date_time < date_from) || (ticket.date_time > date_to));

            return product_check || status_check || date_check;
        }

        private string stringFromList (List<int> list) {

            string output = "";

            foreach (var numb in list)
            {
                output = output + numb + " ";
            }

            return output;
        }
    }
}