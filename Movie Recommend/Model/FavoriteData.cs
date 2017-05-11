using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using SQLite.Net.Interop;
using SQLite.Net.Attributes;
using System.Collections.ObjectModel;

namespace Movie_Recommend.Model
{
    internal class FavoriteData
    {
        public FavoriteData() { }
        public FavoriteData(int ID, string tvTitle, string iconaddress, string grade, string gradeNum, 
                            string data2, string director1, string director2, string star1, string star2,
                            string star3, string star4, string type1, string type2, string type3, string type4
                            , string type5, string storyBrief, string storyMoreLink, string MovieImage, string MovieDiscuss)
        {
            this.UID = ID;
            this.tvTitle = tvTitle;
            this.iconaddress = iconaddress;
            this.grade = grade;
            this.gradeNum = gradeNum;
            this.data2 = data2;
            this.director1 = director1;
            this.director2 = director2;
            this.star1 = star1;
            this.star2 = star2;
            this.star3 = star3;
            this.star4 = star4;
            this.type1 = type1;
            this.type2 = type2;
            this.type3 = type3;
            this.type4 = type4;
            this.type5 = type5;
            this.storyBrief = storyBrief;
            this.storyMoreLink = storyMoreLink;
            this.MovieImage = MovieImage;
            this.MovieDiscuss = MovieDiscuss;
        }
        [PrimaryKey]
        [AutoIncrement]
        [NotNull]
        public int UID { get; set; }
        public string tvTitle { get; set; }
        public string iconaddress { get; set; }
        public string grade { get; set; }
        public string gradeNum { get; set; }
        public string data2 { get; set; }
        public string director1 { get; set; }
        public string director2 { get; set; }
        public string star1 { get; set; }
        public string star2 { get; set; }
        public string star3 { get; set; }
        public string star4 { get; set; }
        public string type1 { get; set; }
        public string type2 { get; set; }
        public string type3 { get; set; }
        public string type4 { get; set; }
        public string type5 { get; set; }
        public string storyBrief { get; set; }
        public string storyMoreLink { get; set; }
        public string MovieImage { get; set; }
        public string MovieDiscuss { get; set; }
    }
}
