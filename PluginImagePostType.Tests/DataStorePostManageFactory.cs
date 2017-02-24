using MyBlogContract;

using MyBlogContract.PostManage;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginImagePostType.Tests
{
    public class DataStorePostManageFactory
    {
        private static IDataStorePostManage _ds;
        public static IDataStorePostManage CurrentDataStore
        {
            get
            {
                if (_ds != null)
                    return _ds;
                else
                    throw new InvalidOperationException("DataStoreFactory not available");
            }
        }
        public static void SetDataStore(IDataStorePostManage DataStore)
        {
            _ds = DataStore;
        }

        public static void SetDataStore()
        {
            _ds = Substitute.For<IDataStorePostManage>();

            IList<IDataStoreRecord> _ds_result
                = new List<IDataStoreRecord>();
            UnicodeEncoding encoding = new UnicodeEncoding();
            IDataStoreRecord record1 = Substitute.For<IDataStoreRecord>();
            record1.PostId = 1;
            record1.PostContentId = 1;
            record1.tempPostContentId = 0;
            record1.Status = IDataStoreRecordStatus.None;
            record1.ContentPluginName = "AnyPlugin";
            record1.ContentPluginVersion = "1.0";
            record1.Comment = "AnyComment";
            MemoryStream ms = new MemoryStream();
            ImageFactory.GetImage(record1.PostContentId).Save(ms, ImageFormat.Bmp);
            record1.ContentData = ms.ToArray();
            record1.IsInGroup = true;
            record1.Order = 1;
            _ds_result.Add(record1);



            IDataStoreRecord record2 = Substitute.For<IDataStoreRecord>();
            record2.PostId = 1;
            record2.PostContentId = 2;
            record2.tempPostContentId = 0;
            record2.Status = IDataStoreRecordStatus.None;
            record2.ContentPluginName = "AnyPlugin";
            record2.ContentPluginVersion = "1.0";
            record2.Comment = "AnyComment2";
            ms = new MemoryStream();
            ImageFactory.GetImage(record2.PostContentId).Save(ms, ImageFormat.Bmp);
            record2.ContentData = ms.ToArray();
            record2.IsInGroup = true;
            record2.Order = 1;
            _ds_result.Add(record2);


            IDataStoreRecord record3 = Substitute.For<IDataStoreRecord>();
            record3.PostId = 2;
            record3.PostContentId = 3;
            record3.tempPostContentId = 0;
            record3.Status = IDataStoreRecordStatus.None;
            record3.ContentPluginName = "AnyPlugin";
            record3.ContentPluginVersion = "1.0";
            record3.Comment = "AnyComment3";
            ms = new MemoryStream();
            ImageFactory.GetImage(record3.PostContentId).Save(ms, ImageFormat.Bmp);

            record3.ContentData = ms.ToArray();
            record3.IsInGroup = false;
            record3.Order = 1;
            _ds_result.Add(record3);




            _ds.GetAllContents().Returns(_ds_result);

            _ds.GetContent(Arg.Any<int>(), Arg.Any<int>()).
             Returns(x => _ds_result
            .Where(r => (int)x[1] == 0
                        ? r.PostContentId == (int)x[0]
                        : r.tempPostContentId == (int)x[1])
            .SingleOrDefault());


            //_ds.WhenForAnyArgs(x => x.Create(Arg.Any<IDataStoreRecord>()))
            //    .Do(x => _ds_result.Add((IDataStoreRecord)x[0]));

            _ds.GetNew().Returns(x => Substitute.For<IDataStoreRecord>());

            _ds.When(x => x.Delete(Arg.Any<int>(), Arg.Any<int>()))
                .Do(x => _ds_result
                            .Remove(_ds_result
                                .Where(r => r.PostContentId == (int)x[0])
                                .SingleOrDefault()));



        }
    }
}
