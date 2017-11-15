using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SilverCard.Rencode.Test
{
    [TestClass]
    public class Test1
    {
        private object EncodeDecode(object obj)
        {
            return Rencode.Decode(Rencode.Encode(obj));
        }

        private void EncodeDecodeAssertAreEqual(object obj)
        {
            Assert.AreEqual(obj, EncodeDecode(obj));
        }

        private void EncodeDecodeCollectionAssertAreEqual(ICollection obj)
        {
           var r = (ICollection)EncodeDecode(obj);
           CollectionAssert.AreEqual(obj, r);
        }



        [TestMethod]
        public void EncodeDecode_Test1()
        {
            EncodeDecodeAssertAreEqual(2.5D);
            EncodeDecodeAssertAreEqual(2.5F);
            EncodeDecodeAssertAreEqual(true);
            EncodeDecodeAssertAreEqual(false);
            EncodeDecodeAssertAreEqual(2);
            EncodeDecodeAssertAreEqual("Test");
            EncodeDecodeAssertAreEqual(new String('0', 128));


            EncodeDecodeCollectionAssertAreEqual(new Dictionary<String, Object>() {
                { "k1", 1 },
                { "k2", "v2" },
                { "k3", null }
            });

            EncodeDecodeCollectionAssertAreEqual(new String[] { "1", "2", "3" });
            EncodeDecodeCollectionAssertAreEqual(new int[] { 1, 2, 3 });
            EncodeDecodeCollectionAssertAreEqual(new Object[] { "1", 2, "3" });

        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void UnsupportedType()
        {
            Rencode.Encode(new TimeSpan(1,1,1));
        }
    }
}
