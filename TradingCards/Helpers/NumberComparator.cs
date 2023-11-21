namespace TradingCards.Helpers
{
    public class NumberComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {


            int num1 = -1, num2 = -1;

            string suffix1 = "", suffix2 = "";

            Int32.TryParse(x, out num1);
            Int32.TryParse(y, out num2);

            if (num1 > 0 && num2 > 0)
            {
                if (num1 > num2)
                    return 1;
                else if (num2 > num1)
                    return -1;
                else
                    return 0;
            }

            else
            {
                var num1Arr = x.Split(" ");
                var num2Arr = y.Split(" ");

                //get number
                Int32.TryParse(num1Arr[0], out num1);
                Int32.TryParse(num2Arr[0], out num2);

                if (num1 > num2)
                    return 1;
                else if (num2 > num1)
                    return -1;
                else
                    return 0;
            }


            return 0;
        }
    }
}
