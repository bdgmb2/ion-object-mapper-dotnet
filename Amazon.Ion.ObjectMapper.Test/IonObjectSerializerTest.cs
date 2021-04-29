using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Amazon.Ion.ObjectMapper.Test.Utils;

namespace Amazon.Ion.ObjectMapper.Test
{
    [TestClass]
    public class IonObjectSerializerTest
    {
        [TestMethod]
        public void SerializesAndDeserializesObjects()
        {
            Check(TestObjects.honda);
        }
        
        [TestMethod]
        public void SerializesAndDeserializesObjectsWithIncludeFields()
        {
            Check(TestObjects.fieldAcademy, new IonSerializationOptions { IncludeFields = true });
        }

        [TestMethod]
        public void SerializesAndDeserializesFields()
        {
            Check(TestObjects.registration);
        }

        [TestMethod]
        public void SerializesAndDeserializesCustomPropertyNames()
        {
            Check(TestObjects.fmRadio);
        }

        [TestMethod]
        public void SerializesAndDeserializesSubtypesBasedOnTypeAnnotations()
        {
            Check(
                new List<Vehicle>()
                {
                    new Plane(), new Boat(), new Helicopter()
                }, 
                new IonSerializationOptions
                {
                    AnnotatedTypeAssemblies = new string[]
                    {
                        typeof(Vehicle).Assembly.GetName().Name
                    }
                }
            );
        }

        [TestMethod]
        public void RespectAnnotationInheritance()
        {
            var serializer = new IonSerializer(new IonSerializationOptions { TypeAnnotationPrefix = new FixedTypeAnnotationPrefix("testing") });
            AssertHasAnnotation("testing.Plane", serializer.Serialize(new Plane()));
            AssertHasNoAnnotations(serializer.Serialize(new Yacht()));
            AssertHasAnnotation("testing.Catamaran", serializer.Serialize(new Catamaran()));
            AssertHasAnnotation("testing.Jet", serializer.Serialize(new Jet()));
            AssertHasAnnotation("testing.Boeing", serializer.Serialize(new Boeing()));
        }

        [TestMethod]
        public void RespectAnnotationPrefixes()
        {
            AssertHasAnnotation(
                "my.prefix.Truck", 
                new IonSerializer(new IonSerializationOptions { IncludeTypeInformation = true, TypeAnnotationPrefix = new FixedTypeAnnotationPrefix("my.prefix") }).Serialize(new Truck()));
            AssertHasAnnotation(
                "my.universal.namespace.BussyMcBusface", 
                new IonSerializer().Serialize(new Bus()));
        }
    }
}