using MyBlogContract;

using MyBlogContract.PostManage;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTextPostType.Tests
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
            record1.ContentData = encoding.GetBytes("MyStringData");
            _ds_result.Add(record1);

            IDataStoreRecord record2 = Substitute.For<IDataStoreRecord>();
            record2.PostId = 1;
            record2.PostContentId = 2;
            record2.tempPostContentId = 0;
            record2.Status = IDataStoreRecordStatus.Modified;
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

            _ds.GetContent(Arg.Any<int>()).
                Returns(x => _ds_result
                        .Where(r => r.PostContentId == (int)x[0])
                        .SingleOrDefault());
            _ds.GetModPost(Arg.Any<int>()).
                Returns(x => _ds_result
                        .Where(r => r.PostId == (int)x[0]));
            _ds.WhenForAnyArgs(x => x.Clear())
                .Do(x => _ds_result.Clear());
                        

        }
    }
}
