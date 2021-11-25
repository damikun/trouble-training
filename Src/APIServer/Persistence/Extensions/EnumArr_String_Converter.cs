using System;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using APIServer.Domain.Core.Models.WebHooks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace APIServer.Persistence.Extensions
{

    public class EnumArrToString_StringToEnumArr_Converter : ValueConverter<HookEventType[], string>
    {

        public EnumArrToString_StringToEnumArr_Converter(
            [NotNullAttribute] Expression<Func<HookEventType[], string>> convertToProviderExpression,
            [NotNullAttribute] Expression<Func<string, HookEventType[]>> convertFromProviderExpression)
                : base(convertToProviderExpression, convertFromProviderExpression)
        {
        }

        public override Expression<Func<HookEventType[], string>> ConvertToProviderExpression
        {
            get
            {

                Expression<Func<HookEventType[], string>> converterExpression = x => Convert(x);

                return converterExpression;
            }
        }

        public override Expression<Func<string, HookEventType[]>> ConvertFromProviderExpression
        {
            get
            {

                Expression<Func<string, HookEventType[]>> converterExpression = x => Convert(x);

                return converterExpression;
            }
        }

        public static string Convert(HookEventType[] sourceMember)
        {
            if (sourceMember == null || !sourceMember.Any())
            {
                return "";
            }
            else
            {

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < sourceMember.Length; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(sourceMember[i].ToString());
                    }
                    else
                    {
                        sb.Append(string.Format(",{0}", sourceMember[i].ToString()));
                    }
                }

                return sb.ToString();
            }
        }

        public static HookEventType[] Convert(string sourceMember)
        {
            var list = new List<HookEventType>();

            if (!String.IsNullOrWhiteSpace(sourceMember))
            {
                sourceMember = sourceMember.Trim();
                foreach (var item in sourceMember.Split(
                    new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct()
                )
                {
                    if (item != null)
                    {
                        try
                        {
                            list.Add(Enum.Parse<HookEventType>(item));
                        }
                        catch { }
                    }
                }
            }

            return list.ToArray();
        }

    }
}
