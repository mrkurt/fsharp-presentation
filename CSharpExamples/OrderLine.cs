using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpExamples
{
    public class OrderLine
    {
        string currName;
        int currQuantity;
        float currPrice;
        public OrderLine(string name, int quantity, float price)
        {
            this.currName = name;
            this.currQuantity = quantity;
            this.currPrice = price;
        }

        public OrderLine(string name, float price)
            : this(name, 1, price)
        { }

        public string Name
        {
            get { return this.currName; }
            set { this.currName = value; }
        }
        public float SubTotal
        {
            get
            {
                return ((float)this.currQuantity) * 
                    this.currPrice;
            }
        }

        public int OneMore()
        {
            return ++this.currQuantity;
        }
    }
}
