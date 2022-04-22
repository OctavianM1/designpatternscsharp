using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Additional;

public static class MayBe
{
    public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> ev)
        where TResult : class 
        where TInput : class
    {
        if (o == null) return null;
        else return ev(o);
    }

    public static TInput If<TInput>(this TInput o, Func<TInput, bool> ev)
        where TInput : class
    {
        if (o == null) return null;
        return ev(o) ? o : null;
    }

    public static TInput Do<TInput>(this TInput o, Action<TInput> action)
        where TInput : class
    {
        if (o == null) return null;
        action(o);
        return o;
    }

    public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> ev, TResult failureValure)
        where TInput : class
    {
        if (o == null) return failureValure;
        return ev(o);
    }

    public static TResult WithValue<TInput, TResult>(TInput o, Func<TInput, TResult> ev)
        where TInput : struct
    {
        return ev(o);
    }
}

public static class MaybeMonad
{
    public class Person
    {
        public Address Address { get; set; }
    }

    public class Address
    {
        public string PostCode { get; set; }
    }

    public static void MyMethod(Person p)
    {
        //string postcode = "UNKNOWN";
        //if (p != null && p.Address != null && p.Address.PostCode != null)
        //    postcode = p.Address.PostCode;

        //postcode = p?.Address?.PostCode;

        //if (p!= null)
        //{
        //    if (HasMedicalRecord(p) && p.Address != null)
        //    {
        //        CheckAddress(p.Address);
        //        if (p.Address.PostCode != null)
        //        {
        //            postcode = p.Address.PostCode;
        //        }
        //        else
        //        {
        //            postcode = "UNKNOWN";
        //        }
        //    }
        //}

        string postcode = p.With(x => x.Address).With(x => x.PostCode);

        postcode = p
            .If(HasMedicalRecord)
            .With(x => x.Address)
            .Do(CheckAddress)
            .Return(x => x.PostCode, "UNKNWON");

        

        
    }

    private static void CheckAddress(Address address)
    {
        throw new NotImplementedException();
    }

    private static bool HasMedicalRecord(Person p)
    {
        throw new NotImplementedException();
    }

    public static void Render()
    {

    }
}
