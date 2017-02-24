using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.PostManage;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Tests
{
    public class DataStoreBandFactory
    {
        private static IDataStoreBand _ds;
        public static IDataStoreBand CurrentDataStore
        {
            get
            {
                if (_ds != null)
                    return _ds;
                else
                    throw new InvalidOperationException("DataStoreFactory not available");
            }
        }
        public static void SetDataStore(IDataStoreBand DataStore)
        {
            _ds = DataStore;
        }

        public static void SetDataStore()
        {
            _ds = Substitute.For<IDataStoreBand>();
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
            record1.ContentData = encoding.GetBytes("MyStringData");
            _ds_result.Add(record1);

            IDataStoreRecord record2 = Substitute.For<IDataStoreRecord>();
            record2.PostId = 1;
            record2.PostContentId = 2;
            record2.tempPostContentId = 0;
            record2.Status = IDataStoreRecordStatus.None;
            record2.ContentPluginName = "AnyPlugin";
            record2.ContentPluginVersion = "1.0";
            record2.Comment = "AnyComment2";
            record2.ContentData = encoding.GetBytes("MyStringData2");
            _ds_result.Add(record2);

            IDataStoreRecord record3 = Substitute.For<IDataStoreRecord>();
            record3.PostId = 2;
            record3.PostContentId = 3;
            record3.tempPostContentId = 0;
            record3.Status = IDataStoreRecordStatus.None;
            record3.ContentPluginName = "AnyPlugin";
            record3.ContentPluginVersion = "1.0";
            record3.Comment = "AnyComment3";
            record3.ContentData = encoding.GetBytes("MyStringData3");
            _ds_result.Add(record3);

            _ds.GetAllContents().Returns(_ds_result);
            //int argumentUsed = 2;
            //var e1 = _ds_result
            //            .Where(r => r.PostContentId == argumentUsed)
            //            .SingleOrDefault();
            _ds.GetContent(Arg.Any<int>()).
                Returns(x => _ds_result
                        .Where(r => r.PostContentId == (int)x[0])
                        .SingleOrDefault());

        }
    }
}
