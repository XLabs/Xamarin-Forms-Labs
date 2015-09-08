#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;

#endif

namespace SerializationTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Serialization;
    using DataObjects;
    using TextSerializationTests;
    using XLabs.Serialization;

    [TestFixture()]
    public abstract class CanSerializerTests
    {
        protected abstract ISerializer Serializer { get; }

        [Test()]
        public void CanSerializePrimitiveAsString()
        {
            var p = Primitives.Create(10);
            Assert.IsTrue(this.Serializer.CanSerializeString<Primitives>(p));
        }

        [Test()]
        public void CanSerializeTuple()
        {
            Tuple<int, string> tuple;

            for (var n = 0; n < 10; n++)
            {
                tuple = new Tuple<int, string>(n, n.ToString());

                
                var str = tuple.ToString();
                Debug.WriteLine(str);

                Assert.IsTrue(this.Serializer.CanSerializeString<Tuple<int, string>>(tuple));
            }
        }

        [Test()]
        public void CanSerializePrimitiveAsBytes()
        {
            var p = Primitives.Create(10);
            Assert.IsTrue(this.Serializer.CanSerializeBytes<Primitives>(p));
        }

        [Test()]
        public void CanSerializePrimitiveAsStream()
        {
            var p = Primitives.Create(10);
            Assert.IsTrue(this.Serializer.CanSerializeStream<Primitives>(p));
        }

        [Test()]
        public void CanSerializePrimitiveListString()
        {
            var list = new PrimitiveList();

            for (var n = 0; n < 10; n++)
            {
                list.List.Add(Primitives.Create(n));
            }

            Assert.IsTrue(this.Serializer.CanSerializeString<PrimitiveList>(list));
        }

        [Test()]
        public void CanSerializePrimitiveListBytes()
        {
            var list = new PrimitiveList();

            for (var n = 0; n < 10; n++)
            {
                list.List.Add(Primitives.Create(n));
            }

            Assert.IsTrue(this.Serializer.CanSerializeBytes<PrimitiveList>(list));
        }

        [Test()]
        public void CanSerializePrimitiveListStream()
        {
            var list = new PrimitiveList();

            for (var n = 0; n < 10; n++)
            {
                list.List.Add(Primitives.Create(n));
            }

            Assert.IsTrue(this.Serializer.CanSerializeStream<PrimitiveList>(list));
        }

        [Test()]
        public void CanSerializeDateTimeAsString()
        {
            var p = DateTime.Now;
            Assert.IsTrue(this.Serializer.CanSerializeString<DateTime>(p));
        }

        [Test()]
        public void CanSerializeDateTimeAsByte()
        {
            var p = DateTime.Now;
            Assert.IsTrue(this.Serializer.CanSerializeBytes<DateTime>(p));
        }

        [Test()]
        public void CanSerializeDateTimeAsStream()
        {
            var p = DateTime.Now;
            Assert.IsTrue(this.Serializer.CanSerializeStream<DateTime>(p));
        }

        [Test()]
        public void CanSerializeDateTimeOffsetAsString()
        {
            var p = new DateTimeOffset(DateTime.Now);
            Assert.IsTrue(this.Serializer.CanSerializeString<DateTimeOffset>(p));
        }

        [Test()]
        public void CanSerializeDateTimeOffsetAsByte()
        {
            var p = new DateTimeOffset(DateTime.Now);
            Assert.IsTrue(this.Serializer.CanSerializeBytes<DateTimeOffset>(p));
        }

        [Test()]
        public void CanSerializeDateTimeOffsetAsStream()
        {
            var p = new DateTimeOffset(DateTime.Now);
            Assert.IsTrue(this.Serializer.CanSerializeStream<DateTimeOffset>(p));
        }

        [Test()]
        public void CanSerializeDates()
        {
            var p = DateTimeDto.Create(101);
            Assert.IsTrue(this.Serializer.CanSerializeString<DateTimeDto>(p));
        }

        [Test()]
        public void CanSerializeSimple()
        {
            var person = new Person()
            {
                Id = 0,
                FirstName = "First",
                LastName = "Last"
            };
            Assert.IsTrue(this.Serializer.CanSerializeString<Person>(person));
        }

        [Test()]
        public void CanSerializeInterface()
        {
            var cat = new Cat()
            {
                Name = "Just some cat"
            };

            Assert.IsTrue(this.Serializer.CanSerializeString<IAnimal>(cat));
        }

        [Test()]
        public void CanSerializeAbstractClass()
        {
            var dog = new Dog()
            {
                Name = "GSP"
            };

            Assert.IsTrue(this.Serializer.CanSerializeString<Animal>(dog));
        }

        [Test()]
        public void CanSerializeListWithInterfaces()
        {
            var animals = new List<IAnimal> { new Cat() { Name = "Just some cat" }, new Dog() { Name = "GSP" } };

            Assert.IsTrue(this.Serializer.CanSerializeEnumerable(animals));
        }

        [Test]
        public void CanSerializeReadOnlyCollection()
        {
            var list = new ReadOnlyList<int>
            {
                Collection = new ReadOnlyCollection<int>(new[] {0, 1, 2})
            };

            Assert.IsTrue(this.Serializer.CanSerializeString(list));
        }

        [DataContract]
        public class PrimitiveList : IEquatable<PrimitiveList>
        {
            public PrimitiveList()
            {
                this.List = new List<Primitives>();
            }

            [DataMember(Order=1)]
            public List<Primitives> List { get; set; }

            #region IEquatable implementation
            public override bool Equals(object obj)
            {
                var list = obj as PrimitiveList;

                return (list != null && this.Equals(list));
            }

            public bool Equals(PrimitiveList other)
            {
                return this.List.SequenceEqual(other.List);
            }

            public override int GetHashCode()
            {
                return this.List.GetHashCode();
            }

            #endregion
        }
    }
}
