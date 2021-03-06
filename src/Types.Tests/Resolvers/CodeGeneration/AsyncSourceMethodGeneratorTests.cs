using System;
using System.Linq;
using System.Reflection;
using System.Text;
using HotChocolate.Resolvers.CodeGeneration;
using Xunit;

namespace HotChocolate.Resolvers
{
    public class AsyncSourceMethodGeneratorTests
    {
        [Fact]
        public void AsyncSourceMethodGenerator_GenerateWithoutArguments()
        {
            // arrange
            var fieldMember = new FieldMember(
                "Foo", "bar",
                GetMethod<GeneratorTestDummy>("GetFooAsync", 0));

            var descriptor = new SourceResolverDescriptor(fieldMember);

            // act
            var source = new StringBuilder();
            var generator = new AsyncSourceMethodGenerator();
            string result = generator.Generate("abc", descriptor);

            // assert
            Assert.Equal(Snapshot.Current(), Snapshot.New(result));
        }


        [Fact]
        public void AsyncSourceMethodGenerator_GenerateWithOneArgument()
        {
            // arrange
            var fieldMember = new FieldMember(
                "Foo", "bar",
                GetMethod<GeneratorTestDummy>("GetFooAsync", 1));

            var descriptor = new SourceResolverDescriptor(fieldMember);

            // act
            var source = new StringBuilder();
            var generator = new AsyncSourceMethodGenerator();
            string result = generator.Generate("abc", descriptor);

            // assert
            Assert.Equal(Snapshot.Current(), Snapshot.New(result));
        }

        [Fact]
        public void AsyncSourceMethodGenerator_GenerateWithTwoArgument()
        {
            // arrange
            var fieldMember = new FieldMember(
                "Foo", "bar",
                GetMethod<GeneratorTestDummy>("GetFooAsync", 2));

            var descriptor = new SourceResolverDescriptor(fieldMember);

            // act
            var source = new StringBuilder();
            var generator = new AsyncSourceMethodGenerator();
            string result = generator.Generate("abc", descriptor);

            // assert
            Assert.Equal(Snapshot.Current(), Snapshot.New(result));
        }

        private MethodInfo GetMethod<T>(string name, int parameters)
        {
            return typeof(T)
                .GetMethods()
                .Single(t => t.Name == name
                    && t.GetParameters().Length == parameters);
        }
    }
}
