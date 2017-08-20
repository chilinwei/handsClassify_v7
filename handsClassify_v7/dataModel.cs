using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace handsClassify_v7
{
    public class settings
    {
        public string camtype { get; set; }
        public string camdevice { get; set; }
        public string savefolder { get; set; }
        public string linkfile { get; set; }
        public string mainpicsfolder { get; set; }

        public List<settings> GetSettings()
        {
            List<settings> query = new List<settings>();
            XDocument xdoc = XDocument.Load(@"config/data/settings.xml");
            List<settings> lst = (from xml in xdoc.Descendants("settings")
                                  select new settings
                                  {
                                      camtype = xml.Element("camtype").Value,
                                      camdevice = xml.Element("camdevice").Value,
                                      savefolder = xml.Element("savefolder").Value,
                                      linkfile = xml.Element("linkfile").Value,
                                      mainpicsfolder = xml.Element("mainpicsfolder").Value
                                  }).ToList();
            return lst;
        }
    }

    public class exam
    {
        public string cno { get; set; }         //檢驗單號碼
        public string idno { get; set; }        //身分證字號
        public string name { get; set; }        //受檢人姓名
        public string sex { get; set; }         //性別(代號)
        public string sex_text { get; set; }    //性別(顯示文字)
        public string age { get; set; }         //年齡
        public string rno { get; set; }         //病歷號碼
        public string ccode { get; set; }       //檢查代碼
        public string ctype { get; set; }       //檢查項目
        public string cnote { get; set; }       //檢查描述
        public string cpart { get; set; }       //檢查部位(代號)
        public string cpart_text { get; set; }  //檢查部位(顯示文字)
        public string ckind { get; set; }       //檢查種類/方向(代號)
        public string ckind_text { get; set; }  //檢查種類/方向(顯示文字)
        public string mainimage { get; set; }   //顯示主要圖片
        public string xrayimage { get; set; }   //顯示Xray圖片
        public string hcode { get; set; }       //醫院代碼
        public string dts { get; set; }         //開始時間
        public string dte { get; set; }         //結束時間
        public string title { get; set; }       //標題
        public string content { get; set; }     //內文        
        public string notice { get; set; }      //注意事項
        public string hname { get; set; }       //醫院名稱
        public string stel { get; set; }        //醫院聯繫電話

        public List<exam> GetCUData(string _cno)
        {
            XDocument xdoc = XDocument.Load(@"config/data/cudata.xml");
            List<exam> lst = (from xml in xdoc.Descendants("form")
                              where xml.Attribute("cno").Value == _cno
                              select new exam
                              {
                                  cno = xml.Attribute("cno").Value,
                                  idno = xml.Element("idno").Value,
                                  name = xml.Element("name").Value,
                                  sex = xml.Element("sex").Value,
                                  age = xml.Element("age").Value,
                                  rno = xml.Element("rno").Value,
                                  ccode = xml.Element("ccode").Value,
                                  ctype = xml.Element("ctype").Value,
                                  cnote = xml.Element("cnote").Value,
                                  cpart = xml.Element("cpart").Value,
                                  ckind = xml.Element("ckind").Value,
                                  hcode = xml.Element("hcode").Value,
                                  dts = xml.Element("dts").Value,
                                  dte = xml.Element("dte").Value,
                                  title = xml.Element("title").Value,
                                  content = xml.Element("content").Value,
                                  notice = xml.Element("notice").Value,
                                  hname = xml.Element("hname").Value,
                                  stel = xml.Element("stel").Value
                              }).ToList();

            var result = lst.Select(i =>
            {
                if (i.sex == "M")
                {
                    i.sex_text = "男性";
                }
                else if (i.sex == "F")
                {
                    i.sex_text = "女性";
                }

                if (i.cpart == "shoulder")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "肩膀"; i.ckind_text = "左邊"; i.mainimage = "R1.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "肩膀"; i.ckind_text = "右邊"; i.mainimage = "L1.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "humerus")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "肱部"; i.ckind_text = "左邊"; i.mainimage = "R2.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "肱部"; i.ckind_text = "右邊"; i.mainimage = "L2.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "elbow")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "手肘"; i.ckind_text = "左邊"; i.mainimage = "R3.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "手肘"; i.ckind_text = "右邊"; i.mainimage = "L3.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "forearm")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "前臂"; i.ckind_text = "左邊"; i.mainimage = "R4.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "前臂"; i.ckind_text = "右邊"; i.mainimage = "L4.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "wrist")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "手腕"; i.ckind_text = "左邊"; i.mainimage = "R5.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "手腕"; i.ckind_text = "右邊"; i.mainimage = "L5.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "hand")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "手掌"; i.ckind_text = "左邊"; i.mainimage = "R6.gif"; i.xrayimage = "handLXray.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "手掌"; i.ckind_text = "右邊"; i.mainimage = "L6.gif"; i.xrayimage = "handRXray.jpg";
                    }
                }
                else if (i.cpart == "hip")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "髖關節"; i.ckind_text = "左邊"; i.mainimage = "R7.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "髖關節"; i.ckind_text = "右邊"; i.mainimage = "L7.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "hip")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "髖關節"; i.ckind_text = "左邊"; i.mainimage = "R7.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "髖關節"; i.ckind_text = "右邊"; i.mainimage = "L7.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "femur")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "股骨"; i.ckind_text = "左邊"; i.mainimage = "R8.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "股骨"; i.ckind_text = "右邊"; i.mainimage = "L8.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "knee")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "膝蓋"; i.ckind_text = "左邊"; i.mainimage = "R9.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "膝蓋"; i.ckind_text = "右邊"; i.mainimage = "L9.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "patella")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "髕骨"; i.ckind_text = "左邊"; i.mainimage = "R10.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "髕骨"; i.ckind_text = "右邊"; i.mainimage = "L10.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "lower_leg")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "小腿"; i.ckind_text = "左邊"; i.mainimage = "R11.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "小腿"; i.ckind_text = "右邊"; i.mainimage = "L11.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "ankle")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "腳踝"; i.ckind_text = "左邊"; i.mainimage = "R12.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "腳踝"; i.ckind_text = "右邊"; i.mainimage = "L12.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "foot")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "腳掌"; i.ckind_text = "左邊"; i.mainimage = "R13.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "腳掌"; i.ckind_text = "右邊"; i.mainimage = "L13.gif"; i.xrayimage = "null.jpg";
                    }
                }
                else if (i.cpart == "calcaneus")
                {
                    if (i.ckind == "L")
                    {
                        i.cpart_text = "足底"; i.ckind_text = "左邊"; i.mainimage = "R14.gif"; i.xrayimage = "null.jpg";
                    }
                    else if (i.ckind == "R")
                    {
                        i.cpart_text = "足底"; i.ckind_text = "右邊"; i.mainimage = "L14.gif"; i.xrayimage = "null.jpg";
                    }
                }
                return i;
            }).ToList();

            return result;
        }
    }
}
