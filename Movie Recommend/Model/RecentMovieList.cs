using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Recommend.Model
{
    public class Rootobject
    {
        public string reason { get; set; }
        public Result result { get; set; }
        public int error_code { get; set; }
    }

    public class Result
    {
        public string title { get; set; }
        public string url { get; set; }
        public string m_url { get; set; }
        public Datum[] data { get; set; }
        public string morelink { get; set; }
        public string date { get; set; }
    }

    public class Datum
    {
        public string name { get; set; }
        public string link { get; set; }
        public ObservableCollection<Datum1> data { get; set; }
    }

    public class Datum1
    {
        public string tvTitle { get; set; }
        public string iconaddress { get; set; }
        public string iconlinkUrl { get; set; }
        public string m_iconlinkUrl { get; set; }
        public string subHead { get; set; }
        public string grade { get; set; }
        public string gradeNum { get; set; }
        public Playdate playDate { get; set; }
        public Director director { get; set; }
        public Star star { get; set; }
        public Type type { get; set; }
        public Story story { get; set; }
        public More more { get; set; }
    }

    public class Playdate
    {
        public string showname { get; set; }
        public string data { get; set; }
        public string data2 { get; set; }
    }

    public class Director
    {
        public string showname { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public A a { get; set; }
        public M_1 m_1 { get; set; }
        public B b { get; set; }
    }

    public class A
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class M_1
    {
        public string link { get; set; }
    }

    public class B
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Star
    {
        public string showname { get; set; }
        public Data1 data { get; set; }
    }

    public class Data1
    {
        public A1 a { get; set; }
        public M_11 m_1 { get; set; }
        public B1 b { get; set; }
        public M_2 m_2 { get; set; }
        public C c { get; set; }
        public M_3 m_3 { get; set; }
        public D d { get; set; }
        public M_4 m_4 { get; set; }
    }

    public class A1
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class M_11
    {
        public string link { get; set; }
    }

    public class B1
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class M_2
    {
        public string link { get; set; }
    }

    public class C
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class M_3
    {
        public string link { get; set; }
    }

    public class D
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class M_4
    {
        public string link { get; set; }
    }

    public class Type
    {
        public string showname { get; set; }
        public Data2 data { get; set; }
    }

    public class Data2
    {
        public A2 a { get; set; }
        public B2 b { get; set; }
        public C1 c { get; set; }
        public D1 d { get; set; }
        public E e { get; set; }
    }

    public class A2
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class B2
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class C1
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class D1
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class E
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Story
    {
        public string showname { get; set; }
        public Data3 data { get; set; }
    }

    public class Data3
    {
        public string storyBrief { get; set; }
        public string storyMoreLink { get; set; }
    }

    public class More
    {
        public Datum2[] data { get; set; }
        public string showname { get; set; }
    }

    public class Datum2
    {
        public string name { get; set; }
        public string link { get; set; }
    }

}
