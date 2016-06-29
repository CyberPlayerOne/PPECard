using System;
using System.Collections.Generic;
using System.Text;

//
using System.Xml;
using System.Data;
using System.Data.OleDb;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PPECard
{
    class ReadXmlIntoDB:IDisposable
    {
        
        private OleDbConnection oleDbCon;
        private string connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "\"" + MainForm.CurrentDirectory + "\\" + "LocalData.mdb" + "\"";
        OleDbCommand cmd;

        //for v1.0 only
        //public DataSet dataSet;
        //public OleDbDataAdapter adapter;
        //private OleDbCommandBuilder commandBuilder;

        private XmlDocument XmlDoc;
        private string xmlString;

        //字段
        string name;
        string cellphone;
        string telephone;
        string qq;
        string email;
        string blogaddress;
        string diploma;
        string title;
        string company;
        string location;
        string displaypic;
        string profile;

        string ip;

        //字段完

        bool GotXmlException;//没有办法的办法

/// <summary>读取XML字符串，添加已经成为好友的用户名片信息到本地数据库.注意！！：数据库连接不是在每个调用的方法中打开和关闭的，而是在最后调用Dispose方法一起关闭的，因此使用本类最后一定要调用Dispose
/// </summary>
/// <param name="XmlString"></param>
/// <param name="Ip">对方IP，用于添加数据库字段</param>
        public ReadXmlIntoDB(string XmlString,string Ip)
        {
            ip = Ip;
             #region test only、、之所以这里测试生成的XML文件不能被IE解析，是因为：作为客户端读取数据的ReadNetStream(TcpClient Client)方法也是用MemoryStream读取并输入到本对象的XmlString的，因此这里也就多了后面的非法空格！！！
            //FileStream fs = new FileStream("客户端收到的XML写入文件.xml", FileMode.Create);
            //StreamWriter wr = new StreamWriter(fs);// System.Windows.Forms.MessageBox.Show(wr.Encoding.ToString()); // UTF8Encoding
            //wr.Write(XmlString);
            //wr.Dispose();
            //fs.Dispose();
             #endregion
            oleDbCon = new OleDbConnection(connectionStr);
            #region 已过时（FOR V.10 ONLY）
            //dataSet = new DataSet();
            //adapter = new OleDbDataAdapter(adapterStr, oleDbCon);
            //adapter.Fill(dataSet, "table");
            //commandBuilder = new OleDbCommandBuilder(adapter);
            #endregion
            //在v2.0中使用XmlReader读取XML;
            //这些DOM对象只用于CheckResponse()函数
            XmlDoc = new XmlDocument();
            GotXmlException = false;
            try//此异常有时出现有时不出现！！！！！！！！！！！！！！！！！
            {
                XmlDoc.LoadXml(XmlString);//?????????????????????????????XmlDocument无法读取含有Base64图片的编码的XML的string???????
                /*未处理 System.Xml.XmlException
      Message="“.”(十六进制值 0x00)是无效的字符。 行 14，位置 3964。"
      Source="System.Xml"
      LineNumber=14
      LinePosition=3964*/
            }
            catch (XmlException) { GotXmlException = true; }
            xmlString = XmlString;
        }
        /// <summary>V2.0+检查返回的XML信息，获知作为服务器的对方是否同意了本人的名片请求
        /// V2.0不需要改动
        /// </summary>
        /// <returns>如果同意了，则为true;如果拒绝，则为false</returns>
        public bool CheckResponse()
        {
            if (GotXmlException)
            { return true; }
            else
            {
                //读取XML
                XmlNode RootNode = XmlDoc.DocumentElement;//应为ECard
                if (RootNode.Name == "ECard") return true;
                else if (RootNode.Name == "DENY") return false;
                else                    
                {
                    string err = "远程用户返回了错误的标志信息:\nRootNode.Name:" + RootNode.Name + "发生未知错误！";
                    System.Windows.Forms.MessageBox.Show(err);
                    MainForm.LogError(MainForm.ErrorLevel.二级严重错误,err);
                    return false;
                }
            }
        }

        /// <summary> 向数据库中插入XMLString里面包含的名片记录
        /// 在V2.0版本中，要同时插入从XML中获取的中文与英文名片+.返回值是数据更新后的用户的用户名，用于修改ListViewItem的Text
        /// </summary>
        public string UpdateOrInsertIntoDB(bool NeedUpdate)
        {
            
            try
            {
                string NameBackup;//用于返回值，可以用于修改ListViewItem的Text（用户名）
                if (NeedUpdate)//应当先删除对应的记录
                {
                    DeleteDBRecord();
                }

                cmd = new OleDbCommand();
                cmd.Connection = oleDbCon;//CommandText需要在后面生成
                if (oleDbCon.State == ConnectionState.Closed) { oleDbCon.Open(); }//如果执行了DeleteDbRecord方法，则如果在这里再Open一下会异常
                //先插入中文名片记录，再英文名片记录到数据库
                string StrCmdInsert;
                System.IO.TextReader strReader = new System.IO.StringReader(xmlString);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                settings.CheckCharacters = false;
                using (XmlReader xmlReader = XmlReader.Create(strReader, settings))
                {//只进、只读
                    while (xmlReader.NodeType != XmlNodeType.Element)
                    { xmlReader.Read(); }//Exceptions:“根级别上的数据无效！”
                    xmlReader.ReadStartElement("ECard");
                    //---------------------------------
                    xmlReader.ReadStartElement("MyCard");
                    xmlReader.ReadStartElement("名字");
                    name = xmlReader.ReadString(); NameBackup = name;
                    xmlReader.ReadEndElement();//名字
                    cellphone = xmlReader.ReadElementString("移动电话");
                    telephone = xmlReader.ReadElementString("固定电话");
                    qq = xmlReader.ReadElementString("QQ");
                    email = xmlReader.ReadElementString("Email");
                    blogaddress = xmlReader.ReadElementString("网址");
                    diploma = xmlReader.ReadElementString("学历");
                    title = xmlReader.ReadElementString("职称");
                    company = xmlReader.ReadElementString("单位名称");
                    location = xmlReader.ReadElementString("地址");
                    //logo字段要保存图片路径，这里要先把图片读出来并保存为文件
                    xmlReader.ReadStartElement("LOGO");
                    displaypic = ip + ".jPg";
                    byte[] buffer = new byte[1024];
                    int count;
                    try//如果
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(MainForm.CurrentDirectory + @"\Images\" + ip + ".jpg", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite, 1024);
                        while ((count = xmlReader.ReadContentAsBase64(buffer, 0, buffer.Length)) > 0)//这里有时也会发生“十六进制值无效异常”
                        {
                            fs.Write(buffer, 0, count);
                            if (count < 30) 
                            { 
                                throw new Exception("没有收到图片编码数据(Chinese)");
                            }//说明收到的这一字段不是图片的base64编码
                        }
                        fs.Flush();
                        fs.Dispose();//自此，图像文件已经保存在Images文件夹下，文件名是[ip].jpg
                    }
                    catch (Exception ex) 
                    { 
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + "请先关闭查看窗体！", "在本机作为客户端创建图片文件时出错(Chinese)"); 
                        xmlReader.Read();
                        displaypic = "";
                    }
                    
                    xmlReader.ReadEndElement();//LOGO
                    profile = xmlReader.ReadElementString("个人简介");
                    xmlReader.ReadEndElement();//MyCard
                    //------------------------------
                    //记录日志
                    MainForm.LogRoutine("成功获取" + name + "的名片信息!");
                    //-------------------------------------
                    //读完中文名片，写入数据库
                    StrCmdInsert = "Insert into MyFriendsData(名字,移动电话,固定电话,QQ,Email,网址,学历,职称,单位名称,地址,LOGO,个人简介,IP) values";
                    StrCmdInsert += "(\"" + name + "\"" + ","
                        + "\"" + cellphone + "\"" + ","
                          + "\"" + telephone + "\"" + ","
                          + "\"" + qq + "\"" + ","
                          + "\"" + email + "\"" + ","
                          + "\"" + blogaddress + "\"" + ","
                          + "\"" + diploma + "\"" + ","
                          + "\"" + title + "\"" + ","
                          + "\"" + company + "\"" + ","
                          + "\"" + location + "\"" + ","
                          + "\"" + displaypic + "\"" + ","
                          + "\"" + profile + "\"" + ","
                          + "\"" + ip + "\""
                          + ")";
                    StrCmdInsert += ";";
                    cmd.CommandText = StrCmdInsert;
                    cmd.ExecuteNonQuery();
                    //------------------------------
                    xmlReader.ReadStartElement("MyCard");
                    xmlReader.ReadStartElement("名字");
                    name = xmlReader.ReadString();
                    xmlReader.ReadEndElement();//名字
                    cellphone = xmlReader.ReadElementString("移动电话");
                    telephone = xmlReader.ReadElementString("固定电话");
                    qq = xmlReader.ReadElementString("QQ");
                    email = xmlReader.ReadElementString("Email");
                    blogaddress = xmlReader.ReadElementString("网址");
                    diploma = xmlReader.ReadElementString("学历");
                    title = xmlReader.ReadElementString("职称");
                    company = xmlReader.ReadElementString("单位名称");
                    location = xmlReader.ReadElementString("地址");
                    //logo字段要保存图片路径，这里要先把图片读出来并保存为文件
                    xmlReader.ReadStartElement("LOGO");
                    displaypic = ip + "~.jpG";
                    byte[] buffer2 = new byte[1024];
                    int count2;
                    try
                    {
                        System.IO.FileStream fs2 = new System.IO.FileStream(MainForm.CurrentDirectory + @"\Images\" + ip + "~.jpg", System.IO.FileMode.Create);
                        while ((count2 = xmlReader.ReadContentAsBase64(buffer2, 0, buffer2.Length)) > 0)
                        {
                            fs2.Write(buffer2, 0, count2);
                            if (count2 < 30)
                            {
                                throw new Exception("没有收到图片编码数据(English)");
                            }
                        }
                        fs2.Flush();
                        fs2.Dispose();//自此，图像文件已经保存在Images文件夹下，文件名是[ip].jpg
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + "请先关闭查看窗体！", "在本机作为客户端创建图片文件时出错(English)");
                        xmlReader.Read();
                        displaypic = "";
                    }
                    
                    xmlReader.ReadEndElement();//LOGO
                    profile = xmlReader.ReadElementString("个人简介");
                    xmlReader.ReadEndElement();//MyCard
                    //读完英文名片，写入数据库
                    StrCmdInsert = "Insert into MyFriendsDataEn(名字,移动电话,固定电话,QQ,Email,网址,学历,职称,单位名称,地址,LOGO,个人简介,ip) values";
                    StrCmdInsert += "(\"" + name + "\"" + ","
                        + "\"" + cellphone + "\"" + ","
                          + "\"" + telephone + "\"" + ","
                          + "\"" + qq + "\"" + ","
                          + "\"" + email + "\"" + ","
                          + "\"" + blogaddress + "\"" + ","
                          + "\"" + diploma + "\"" + ","
                          + "\"" + title + "\"" + ","
                          + "\"" + company + "\"" + ","
                          + "\"" + location + "\"" + ","
                          + "\"" + displaypic + "\"" + ","
                          + "\"" + profile + "\"" + ","
                          + "\"" + ip + "\""
                          + ")";
                    StrCmdInsert += ";";
                    cmd.CommandText = StrCmdInsert;
                    cmd.ExecuteNonQuery();
                    //---------------------------------
                    // xmlReader.ReadEndElement();//ECard--->在 SendMyCard(NetworkStream networkStream)方法中使用TextWriter后，可以读取了.XML中仍然有空格存在。在接收时被替换去了。                   
                }
                oleDbCon.Dispose();
                strReader.Dispose();

                return NameBackup;

                #region 已过时 for v1.0 only
                //读取XML
                //XmlNode ECardNode = XmlDoc.DocumentElement;//应为ECard
                //XmlNode MyCardNode = ECardNode.FirstChild;//MyCard            
                //  //更新DataSet
                //  DataRow NewRow = dataSet.Tables["table"].NewRow();
                //  //NewRow["名字"] = "";
                //  XmlElement XmlElem = (XmlElement)MyCardNode.FirstChild;//名字
                //  do
                //  {
                //      if (XmlElem.Name == "名字") { name = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //      if (XmlElem.Name == "移动电话") { cellphone = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //      if (XmlElem.Name == "固定电话") { telephone = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //      if (XmlElem.Name == "QQ") { qq = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //      if (XmlElem.Name == "Email") { email = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //      if (XmlElem.Name == "网址") { blogaddress = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //      if (XmlElem.Name == "地址") { location = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //      if (XmlElem.Name == "LOGO") { displaypic = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //      //原来的数据库的“LOGO”字段，现在在数据库中应该为本地路径（暂不改字段名），在XML中要读出base64编码
                //      //因此在这里的操作应该是base64解码，存储图片文件，保存路径到本地数据库中
                //      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //      if (XmlElem.Name == "个人简介") { profile = getNodeText(XmlElem); XmlElem = (XmlElement)XmlElem.NextSibling; continue; }
                //  } while (XmlElem!=null);

                //  //在这里用XmlReader读取头像节点的文本内容Base64编码，用ReadContentAsBase64方法和FileStream对象生成对应的图像文件，
                //  //并存在/Image/目录下，再把头像文件的完全路径赋给displaypic字段
                //  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //  NewRow["名字"] = name;
                //  NewRow["移动电话"] = cellphone;
                //  NewRow["固定电话"] = telephone;
                //  NewRow["QQ"] = qq;
                //  NewRow["Email"] = email;
                //  NewRow["网址"] = blogaddress;
                //  NewRow["地址"] = location;
                //  NewRow["LOGO"] = displaypic;//已经存好的图片的路径
                //  NewRow["个人简介"] = profile;
                //  NewRow["IP"] = ip;

                //  dataSet.Tables[0].Rows.Add(NewRow);
                //  adapter.Update(dataSet, "table");

                ////  System.Windows.Forms.MessageBox.Show("添加成功！");//test only
                #endregion

                //V2.0:使用XmlReader读取XML并存入数据库中
            }
            catch (Exception ex) 
            { 
                System.Windows.Forms.MessageBox.Show(ex.Message, "XmlReader Exception");
                MainForm.LogError(MainForm.ErrorLevel.一级灾难错误, ex.Message);
                return "";//修改Text属性时要判断
            }
            finally { if (oleDbCon != null) oleDbCon.Close();  }           
        }

        #region 此函数已过时
        //private string getNodeText(XmlNode root)
        //{
        //    if (root is XmlElement)
        //    {
        //        if (root.HasChildNodes)
        //        {
        //            return getNodeText(root.FirstChild);
        //        }
        //        return "有错误发生1:没有孩子节点！";
        //    }
        //    if (root is XmlText)
        //    {
        //        return ((XmlText)root).Value;
        //    }
        //    return "有错误发生2：为空！";
        //}
        #endregion
        /// <summary>V2.0+     在数据库中删除与ip对应的记录.只有在CheckResponse()调用后返回真（说明发过来的是名片），
        /// 并且CheckIsFriend函数返回真（说明已经至少有中文名片存在于本地数据库中）的情况下，才能调用！
        /// 
        /// 在V2.0版本中，要同时删除中文与英文数据库：中文记录正常删除，由于英文记录有可能不存在，故需尝试删除，并且如果不存在则捕获异常并且不作反应
        /// </summary>
        private void DeleteDBRecord()
        {
           // oleDbCon = new OleDbConnection(connectionStr);
            oleDbCon.Open();

            try
            {
                cmd = new OleDbCommand("delete from " + "MyFriendsData" + " where IP=\"" + ip + "\"", oleDbCon);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message,"中文名片删除出错"); }

            try
            {
                cmd = new OleDbCommand("delete from " + "MyFriendsDataEn" + " where IP=\"" + ip + "\"", oleDbCon);
                cmd.ExecuteNonQuery();
            }
            catch { }//,如果有异常则本来就没有英文记录，故不作反应

           // if (oleDbCon != null) oleDbCon.Dispose(); 
            #region 已过时
            //--------------第一次是这样写的，为什么不行？？---------------------------------------
            //DataColumn[] keys = new DataColumn[1];
            //keys[0] = dataSet.Tables["MyFriendsData"].Columns["ID"];//修改字段名成ID了
            //dataSet.Tables["MyFriendsData"].PrimaryKey = keys;//指定主键
            //DataRow findRow = null;
            //foreach (DataRow row in dataSet.Tables["MyFriendsData"].Rows)//找到对应行
            //{
            //    if (row["IP"].ToString() == ip)
            //    {
            //        findRow = row; break;
            //    }
            //}
            //if (findRow != null)
            //{
            //    findRow.Delete();
            //    //  adapter.Update(dataSet, "MyFriendsData");
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            //---------------------------------------------------------------------------------
            #endregion
        }
       





        #region IDisposable 成员
        /// <summary>
        /// 释放所有用过的资源
        /// </summary>
        public void Dispose()
        {
            if (oleDbCon != null) oleDbCon.Dispose();
            //if (dataSet != null) dataSet.Dispose();
            //if (adapter != null) adapter.Dispose();
            //if (commandBuilder != null) commandBuilder.Dispose();
        }

        #endregion
    }
}
