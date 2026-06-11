using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Reports.Abstract;

namespace BusinessObject.Reports.Concrete
{
    internal sealed class InrNumberToWordsConvertor : NumberToWordsConvertor
    {
        public InrNumberToWordsConvertor(string currency) : base(currency) { }
        public override string ConvertNumberToWords(string Number)
        {
            string Temp = string.Empty;
            string Rupees = string.Empty, Paisa = string.Empty;
            int DecimalPlace, iCount;
            string Hundreds = string.Empty;
            string Words = string.Empty;

            string[] place = new string[9];
            place[0] = " Thousand ";
            place[2] = " Lakh ";
            place[4] = " Crore ";
            place[6] = " Arab ";
            place[8] = " Kharab ";

            string MyNumber = Number.ToString().Trim();

            // Find decimal place.
            DecimalPlace = Instr(MyNumber, ".");

            //If we find decimal place...
            if (DecimalPlace > 0)
            {
                // Convert Paisa
                string tt = Mid(MyNumber, DecimalPlace + 1) + "00";
                Temp = Left(tt, 2);
                if (Convert.ToInt32(Temp) > 0)
                {
                    Paisa = " and " + ConvertTens(Temp) + " Paisa";
                }
                // Strip off paisa from remainder to convert.
                MyNumber = Left(MyNumber, DecimalPlace - 1).Trim();
            }

            //===============================================================
            string TM = ""; // If MyNumber between Rs.1 To 99 Only.
            TM = Right(MyNumber, 2);

            if (MyNumber.Length > 0 && MyNumber.Length <= 2)
            {
                if (MyNumber.Length == 1)
                {
                    Words = ConvertDigit(TM);
                    //RupeesToWord = "Rupees " & Words & Paisa & " Only"
                    return Words + Paisa + " Only";
                }
                else
                {
                    if (TM.Length == 2)
                    {
                        Words = ConvertTens(TM);
                        //RupeesToWord = "Rupees " & Words & Paisa & " Only"
                        return Words + Paisa + " Only";
                    }
                }
            }

            // Convert last 3 digits of MyNumber to ruppees in word.
            Hundreds = ConvertHundreds(Right(MyNumber, 3));
            // Strip off last three digits
            MyNumber = Left(MyNumber, MyNumber.Length - 3);

            iCount = 0;
            while (!string.IsNullOrWhiteSpace(MyNumber))
            {
                Temp = Right(MyNumber, 2);
                if (MyNumber.Length == 1)
                {
                    switch (Words.Trim())
                    {
                        case "Thousand":
                        case "Lakh Thousand":
                        case "Lakh":
                        case "Crore":
                        case "Crore Lakh Thousand":
                        case "Arab Crore Lakh Thousand":
                        case "Arab":
                        case "Kharab Arab Crore Lakh Thousand":
                        case "Kharab":
                            Words = ConvertDigit(Temp) + place[iCount];
                            MyNumber = Left(MyNumber, MyNumber.Length - 1);
                            break;
                        default:
                            Words = ConvertDigit(Temp) + place[iCount] + Words;
                            MyNumber = Left(MyNumber, MyNumber.Length - 1);
                            break;
                    }
                }
                else
                {
                    switch (Words.Trim())
                    {
                        case "Thousand":
                        case "Lakh Thousand":
                        case "Lakh":
                        case "Crore":
                        case "Crore Lakh Thousand":
                        case "Arab Crore Lakh Thousand":
                        case "Arab":
                            Words = ConvertTens(Temp) + place[iCount];
                            MyNumber = Left(MyNumber, MyNumber.Length - 2);
                            break;
                        default:
                            if ((Words + place[iCount]).ToString().Trim() == "Lakh" ||
                                (Words + place[iCount]).ToString().Trim() == "Crore" ||
                                (Words + place[iCount]).ToString().Trim() == "Arab")
                            {
                                MyNumber = Left(MyNumber, MyNumber.Length - 2);
                            }
                            else
                            {
                                Words = ConvertTens(Temp) + place[iCount] + Words;
                                MyNumber = Left(MyNumber, MyNumber.Length - 2);
                            }
                            break;
                    }
                }
                iCount += 2;
            }

            //RupeesToWord = "Rupees " & Words & Hundreds & Paisa & " Only"
            return "Rupees " + Words + Hundreds + Paisa + " Only";
        }

        #region Helper Methods
        private string ConvertHundreds(string MyNumber)
        {
            string Result = string.Empty;

            //Exit if there is nothing to convert.
            if (Convert.ToInt32(MyNumber) == 0) return Result;

            // Append leading zeros to number.
            MyNumber = Right("000" + MyNumber, 3);

            // Do we have a hundreds place digit to convert?
            if (Left(MyNumber, 1) != "0")
            {
                Result = ConvertDigit(Left(MyNumber, 1)) + " Hundred ";
            }

            //Do we have a tens place digit to convert?
            if (Mid(MyNumber, 2, 1) != "0")
            {
                Result = Result + ConvertTens(Mid(MyNumber, 2));
            }
            else
            {
                // If not, then convert the ones place digit.
                Result = Result + ConvertDigit(Mid(MyNumber, 3));
            }
            return Result.Trim();
        }

        private string ConvertTens(string MyTens)
        {
            string Result = string.Empty;
            //Is value between 10 and 19?
            if (Convert.ToInt32(Left(MyTens, 1)) == 1)
            {
                switch (MyTens)
                {
                    case "10": Result = "Ten"; break;
                    case "11": Result = "Eleven"; break;
                    case "12": Result = "Twelve"; break;
                    case "13": Result = "Thirteen"; break;
                    case "14": Result = "Fourteen"; break;
                    case "15": Result = "Fifteen"; break;
                    case "16": Result = "Sixteen"; break;
                    case "17": Result = "Seventeen"; break;
                    case "18": Result = "Eighteen"; break;
                    case "19": Result = "Nineteen"; break;
                    default:
                        break;
                }
            }
            else
            {
                // .. otherwise it's between 20 and 99.
                switch (Left(MyTens, 1))
                {
                    case "2": Result = "Twenty "; break;
                    case "3": Result = "Thirty "; break;
                    case "4": Result = "Forty "; break;
                    case "5": Result = "Fifty "; break;
                    case "6": Result = "Sixty "; break;
                    case "7": Result = "Seventy "; break;
                    case "8": Result = "Eighty "; break;
                    case "9": Result = "Ninety "; break;
                    default:
                        break;
                }
                //Convert ones place digit.
                Result = Result + ConvertDigit(Right(MyTens, 1));
            }
            return Result;
        }

        private string ConvertDigit(string MyDigit)
        {
            switch (MyDigit)
            {
                case "1": return "One";
                case "2": return "Two";
                case "3": return "Three";
                case "4": return "Four";
                case "5": return "Five";
                case "6": return "Six";
                case "7": return "Seven";
                case "8": return "Eight";
                case "9": return "Nine";
                default: return "";
            }
        }
        #endregion

        #region Common VB String Functions

        private string Mid(string str, int start)
        {
            return str.Substring(start - 1);
        }

        private string Mid(string str, int start, int length)
        {
            if (start - 1 < 0)
            {
                throw new ArgumentException("StartIndex must be greater than or equal to Zero");
            }
            return str.Substring(start - 1, length);
        }

        private string Left(string str, int length)
        {
            return str.Substring(0, length);
        }

        private string Right(string str, int length)
        {
            return str.Length < length ? str : str.Substring(str.Length - length);
        }

        private int Instr(string string1, string string2)
        {
            return string1.IndexOf(string2) + 1;
        }

        #endregion
    }
}
