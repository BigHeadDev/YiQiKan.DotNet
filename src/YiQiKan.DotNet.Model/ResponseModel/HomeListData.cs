using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class HomeListData {
        public Topbanner[] topBanners { get; set; }
        public Admarquee[] adMarquee { get; set; }
        public Adinlistbanner[] adInListBanners { get; set; }
        public bool isHaveTopBanners { get; set; }
        public Resource[] resources { get; set; }
    }

    public class Topbanner {
        public string id { get; set; }
        public string title { get; set; }
        public string imgUrl { get; set; }
        public string picJuHe { get; set; }
        public string type { get; set; }
        public int sort { get; set; }
        public string reminder { get; set; }
        public string movieId { get; set; }
    }

    public class Admarquee {
        public string id { get; set; }
        public string reminder { get; set; }
        public string type { get; set; }
        public string linkAddress { get; set; }
        public int sort { get; set; }
        public string movieId { get; set; }
    }

    public class Adinlistbanner {
        public string typeName { get; set; }
        public Recommenddto[] recommendDtos { get; set; }
    }

    public class Recommenddto {
        public string id { get; set; }
        public string recommend { get; set; }
        public string module { get; set; }
        public string imgUrl { get; set; }
        public string picJuHe { get; set; }
        public string type { get; set; }
        public string movieId { get; set; }
    }

    public class Resource {
        public string typeName { get; set; }
        public VideoItem[] datalist { get; set; }
    }

}
