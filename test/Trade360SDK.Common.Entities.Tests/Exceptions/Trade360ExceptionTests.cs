using System;
using System.Collections.Generic;
using System.Linq;
using Trade360SDK.Common.Exceptions;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests
{
    public class Trade360ExceptionTests
    {
        [Fact]
        public void Constructor_WithErrors_SetsErrorsProperty()
        {
            var errors = new List<string?> { "Error1", "Error2" };
            var ex = new Trade360Exception(errors);
            Assert.Equal(errors, ex.Errors);
        }

        [Fact]
        public void Constructor_WithMessageAndErrors_SetsProperties()
        {
            var errors = new List<string?> { "Error1" };
            var ex = new Trade360Exception("msg", errors);
            Assert.Equal(errors, ex.Errors);
            Assert.Contains("Error1", ex.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndRawErrorResponse_FormatsMessage()
        {
            var ex = new Trade360Exception("msg", "raw");
            Assert.Contains("msg: raw", ex.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsInnerException()
        {
            var inner = new Exception("inner");
            var ex = new Trade360Exception("msg", inner);
            Assert.Equal(inner, ex.InnerException);
        }

        [Fact]
        public void Message_Returns_JoinedErrors_IfPresent()
        {
            var errors = new List<string?> { "A", "B" };
            var ex = new Trade360Exception(errors);
            Assert.Equal("A; B", ex.Message);
        }

        [Fact]
        public void Constructor_WithMessage_SetsMessage()
        {
            var ex = new Trade360Exception("test message", new List<string?>());
            Assert.Equal("test message", ex.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsProperties()
        {
            var inner = new Exception("inner");
            var ex = new Trade360Exception("outer", inner);
            Assert.Equal("outer", ex.Message);
            Assert.Equal(inner, ex.InnerException);
        }
    }
} 