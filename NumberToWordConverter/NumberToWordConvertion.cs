using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.CommonManagement;
using System.Data;
using System.Security;

[assembly: AllowPartiallyTrustedCallers]
namespace NumberToWordConverter
{
  public class NumberToWordConvertion
    {
      DataTable dtCurrency;
      public string ConvertNumberToWords(string Number)
      {
          string AmountInWords = string.Empty;
          try
          {

                //dtCurrency = CommonDA.GetCurrencyConfiguration("NUMBER TO WORDS", "");

                //if (dtCurrency != null && dtCurrency.Rows.Count > 1)
                //{
                //    int Acf_Value = Convert.ToInt32(dtCurrency.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "TYPE")["ACF_VALUE"].ToString());
                //    int OnlyReq = Convert.ToInt32(dtCurrency.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ONLY REQD FOR DEC")["ACF_VALUE"].ToString());
                //    if (Acf_Value == 1 && OnlyReq == 1)
                //    {
                //        AmountInWords = LocalCurrencyConversionWithOnly(Number);
                //    }
                //    if (Acf_Value == 1 && OnlyReq == 0)
                //    {
                //        AmountInWords = LocalCurrencyConversionWithOnly(Number);
                //    }
                //    if (Acf_Value == 2 && OnlyReq == 1)
                //    {
                //        AmountInWords = ForeignCurrencyConversionDecimalWithOnly(Number);
                //    }
                //    if (Acf_Value == 2 && OnlyReq == 0)
                //    {
                //        AmountInWords = ForeignCurrencyConversionDecimalWithOutOnly(Number);
                //    }

                // }
                AmountInWords = ForeignCurrencyConversionDecimalWithOutOnly(Number);
            }
          catch (Exception e)
          {
              AmountInWords = e.InnerException.ToString();
          }
          return AmountInWords;


      }
      #region Thai Converter
      #region Private Variables
      private static string s1 = "";
      private static string s2 = "";
      private static string s3 = "";
      private static string[] suffix = { "", "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
      private static string[] numSpeak = { "", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };
      #endregion
      #region Helper Methods
      public string ToBahtText(double value)
      {
          string result;

          if (value == 0) return ("");

          splitCurr(value);
          result = "";
          if (s1.Length > 0)
          {
              result = result + Speak(s1) + "ล้าน";
          }
          if (s2.Length > 0)
          {
              result = result + Speak(s2) + "บาท";
          }
          if (s3.Length > 0)
          {
              result = result + speakStang(s3) + "สตางค์";
          }
          else
          {
              result = result + "ถ้วน";
          }
          return (result);
      }

      private static string Speak(string s)
      {
          int L, c;
          string result;

          if (s == "") return ("");

          result = "";
          L = s.Length;
          for (int i = 0; i < L; i++)
          {
              if ((s.Substring(i, 1) == "-"))
              {
                  result = result + "ติดลบ";
              }
              else
              {
                  c = System.Convert.ToInt32(s.Substring(i, 1));
                  if ((i == L - 1) && (c == 1))
                  {
                      if (L == 1)
                      {
                          return ("หนึ่ง");
                      }
                      if ((L > 1) && (s.Substring(L - 1, 1) == "0"))
                      {
                          result = result + "หนึ่ง";
                      }
                      else
                      {
                          result = result + "เอ็ด";
                      }
                  }
                  else if ((i == L - 2) && (c == 2))
                  {
                      result = result + "ยี่สิบ";
                  }
                  else if ((i == L - 2) && (c == 1))
                  {
                      result = result + "สิบ";
                  }
                  else
                  {
                      if (c != 0)
                      {
                          result = result + numSpeak[c] + suffix[L - i];
                      }
                  }
              }
          }
          return (result);
      }

      private static string speakStang(string s)
      {
          int L, c;
          string result;

          L = s.Length;

          if (L == 0) return ("");

          if (L == 1)
          {
              s = s + "0";
              L = 2;
          }
          if (L > 2)
          {
              s = s.Substring(0, 2);
              L = 2;
          }
          result = "";
          for (int i = 0; i < 2; i++)
          {
              c = Convert.ToInt32(s.Substring(i, 1));
              if ((i == L - 1) && (c == 1))
              {
                  if (Convert.ToInt32(s.Substring(0, 1)) == 0)
                      result = result + "หนึ่ง";
                  else
                      result = result + "เอ็ด";
              }
              else if ((i == L - 2) && (c == 2))
              {
                  result = result + "ยี่สิบ";
              }
              else if ((i == L - 2) && (c == 1))
              {
                  result = result + "สิบ";
              }
              else
              {
                  if (c != 0)
                  {
                      result = result + numSpeak[c] + suffix[L - i];
                  }
              }
          }

          return (result);
      }

      private static void splitCurr(double m)
      {
          string s;
          int L;
          int position;

          s = System.Convert.ToString(m);
          position = s.IndexOf(".");
          if ((position >= 0))
          {
              s1 = s.Substring(0, position);
              s3 = s.Substring(position + 1);
              if (s3 == "00")
              {
                  s3 = "";
              }
          }
          else
          {
              s1 = s;
              s3 = "";
          }
          L = s1.Length;
          if ((L > 6))
          {
              s2 = s1.Substring(L - 6);
              s1 = s1.Substring(0, L - 6);
          }
          else
          {
              s2 = s1;
              s1 = "";
          }

          if ((s1 != "") && (Convert.ToInt32(s1) == 0)) s1 = "";
          if ((s2 != "") && (Convert.ToInt32(s2) == 0)) s2 = "";
      }
      #endregion
      #endregion
      #region Helper Methods

      public string LocalCurrencyConversionWithOnly(string Number)
      {
          string Temp = string.Empty;
          string Rupees = string.Empty, Paise = string.Empty;
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
              // Convert Paise
              string tt = Mid(MyNumber, DecimalPlace + 1) + "00";
              Temp = Left(tt, 2);
              if (Convert.ToInt32(Temp) > 0)
              {
                  Paise = " and " + ConvertTens(Temp) + " Paise";
              }
              // Strip off Paise from remainder to convert.
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
                  //RupeesToWord = "Rupees " & Words & Paise & " Only"
                  return Words + Paise + " Only";
              }
              else
              {
                  if (TM.Length == 2)
                  {
                      Words = ConvertTens(TM);
                      //RupeesToWord = "Rupees " & Words & Paise & " Only"
                      return Words + Paise + " Only";
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

          //RupeesToWord = "Rupees " & Words & Hundreds & Paise & " Only"
          return "Rupees " + Words + Hundreds + Paise + " Only";
      }

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

      #region Foreign Currency
      public String ForeignCurrencyConversionDecimalWithOnly(String numb)
      {
          bool isCurrency = true;
          String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
          String endStr = (isCurrency) ? ("Only") : ("");
          try
          {
              int decimalPlace = numb.IndexOf(".");
              if (decimalPlace > 0)
              {
                  wholeNo = numb.Substring(0, decimalPlace);
                  points = numb.Substring(decimalPlace + 1);
                  if (Convert.ToInt32(points) > 0)
                  {
                      andStr = (isCurrency) ? ("and ") : ("point");// just to separate whole numbers from points/Rupees
                      endStr = (isCurrency) ? (" " + endStr) : ("");
                      pointStr = translateRupees(points);
                  }
              }
              val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
          }
          catch
          {
              ;
          }
          return val;
      }
      public String ForeignCurrencyConversionDecimalWithOutOnly(String numb)
      {
          numb = numb.Replace(",", "");
          bool isCurrency = true;
          String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
          String endStr = "Only";
          try
          {
              int decimalPlace = numb.IndexOf(".");
              if (decimalPlace > 0)
              {
                  
                  wholeNo = numb.Substring(0, decimalPlace);
                  points = numb.Substring(decimalPlace + 1);
                  if (Convert.ToInt32(points) > 0)
                  {
                      endStr = "";
                  }
                  if (Convert.ToInt32(points) > 0)
                  {
                      andStr = (isCurrency) ? ("and ") : ("point");// just to separate whole numbers from points/Rupees
                      endStr = (isCurrency) ? (" " + endStr) : ("");
                      pointStr = translateRupees(points);
                  }

              }
              val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);

          }
          catch
          {
              ;
          }
          return val;
      }

      
      private String translateWholeNumber(String number)
      {
          string word = "";
          try
          {
              bool beginsZero = false;//tests for 0XX
              bool isDone = false;//test if already translated
              double dblAmt = (Convert.ToDouble(number));
              //if ((dblAmt > 0) && number.StartsWith("0"))

              if (dblAmt > 0)
              {//test for zero or digit zero in a nuemric
                  beginsZero = number.StartsWith("0");
                  int numDigits = number.Length;
                  int pos = 0;//store digit grouping
                  String place = "";//digit grouping name:hundres,thousand,etc...
                  switch (numDigits)
                  {
                      case 1://ones' range
                          word = ones(number);
                          isDone = true;
                          break;
                      case 2://tens' range
                          word = tens(number);
                          isDone = true;
                          break;
                      case 3://hundreds' range
                          pos = (numDigits % 3) + 1;
                          place = number.Substring(0, pos) == "0" ? " " : " Hundred ";
                          break;
                      case 4://thousands' range
                      case 5:
                      case 6:
                          pos = (numDigits % 4) + 1;
                          place = number.Substring(0, 3) == "000" ? " " : " Thousand ";
                          break;
                      case 7://millions' range
                      case 8:
                      case 9:
                          pos = (numDigits % 7) + 1;
                          place = number.Substring(0, 3) == "000" ? " " : " Million ";
                          break;
                      case 10://Billions's range
                          pos = (numDigits % 10) + 1;
                          place = " Billion ";
                          break;
                      //add extra case options for anything above Billion...
                      default:
                          isDone = true;
                          break;
                  }
                  if (!isDone)
                  {//if transalation is not done, continue...(Recursion comes in now!!)
                      word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));
                      //check for trailing zeros
                      //if (beginsZero) word = (word.Substring(0, 3) == "and" ? "" : " and ") + word.Trim();

                  }
                  //ignore digit grouping names
                  if (word.Trim().Equals(place.Trim())) word = "";
              }
          }
          catch
          {
              ;
          }
          return word.Trim();
      }

      private String tens(String digit)
      {
          int digt = Convert.ToInt32(digit);
          String name = null;
          switch (digt)
          {
              case 10:
                  name = "Ten";
                  break;
              case 11:
                  name = "Eleven";
                  break;
              case 12:
                  name = "Twelve";
                  break;
              case 13:
                  name = "Thirteen";
                  break;
              case 14:
                  name = "Fourteen";
                  break;
              case 15:
                  name = "Fifteen";
                  break;
              case 16:
                  name = "Sixteen";
                  break;
              case 17:
                  name = "Seventeen";
                  break;
              case 18:
                  name = "Eighteen";
                  break;
              case 19:
                  name = "Nineteen";
                  break;
              case 20:
                  name = "Twenty";
                  break;
              case 30:
                  name = "Thirty";
                  break;
              case 40:
                  name = "Forty";
                  break;
              case 50:
                  name = "Fifty";
                  break;
              case 60:
                  name = "Sixty";
                  break;
              case 70:
                  name = "Seventy";
                  break;
              case 80:
                  name = "Eighty";
                  break;
              case 90:
                  name = "Ninety";
                  break;
              default:
                  if (digt > 0)
                  {
                      name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
                  }
                  break;
          }
          return name;
      }

      private String ones(String digit)
      {
          int digt = Convert.ToInt32(digit);
          String name = "";
          switch (digt)
          {
              case 1:
                  name = "One";
                  break;
              case 2:
                  name = "Two";
                  break;
              case 3:
                  name = "Three";
                  break;
              case 4:
                  name = "Four";
                  break;
              case 5:
                  name = "Five";
                  break;
              case 6:
                  name = "Six";
                  break;
              case 7:
                  name = "Seven";
                  break;
              case 8:
                  name = "Eight";
                  break;
              case 9:
                  name = "Nine";
                  break;
          }
          return name;
      }

      private String translateRupees(String Rupees)
      {

          string Temp = string.Empty, Paise = string.Empty;
          int DecimalPlace = Instr(Rupees, ".");
          string tt = Mid(Rupees, DecimalPlace + 1) + "00";
          Temp = Left(tt, 2);
          if (Convert.ToInt32(Temp) > 0)
          {
              Paise = tens(Temp) + " Paise";
          }
          return Paise;
      }

      #endregion
    }
}
