using MvcTemplate.Components.Security;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class BCrypterTests
    {
        private BCrypter crypter;

        public BCrypterTests()
        {
            crypter = new BCrypter();
        }

        #region Method: Hash(String value)

        [Fact]
        public void Hash_Hashes()
        {
            String value = "Test";
            String hash = crypter.Hash(value);

            Assert.True(BCrypt.Net.BCrypt.Verify(value, hash));
        }

        #endregion

        #region Method: HashPassword(String value)

        [Fact]
        public void HashPassword_Hashes()
        {
            String value = "Test";
            String hash = crypter.HashPassword(value);

            Assert.True(BCrypt.Net.BCrypt.Verify(value, hash));
        }

        #endregion

        #region Method: Verify(String value, String hash)

        [Fact]
        public void Verify_OnNullValueFails()
        {
            Assert.False(crypter.Verify(null, ""));
        }

        [Fact]
        public void Verify_OnNullHashFails()
        {
            Assert.False(crypter.Verify("", null));
        }

        [Fact]
        public void Verify_VerifiesHash()
        {
            Assert.True(crypter.Verify("Test", "$2a$04$tXfDH9cZGOqFbCV8CF1ik.kW8R7.UKpEi5G7P4K842As1DI1bwDxm"));
        }

        #endregion

        #region Method: VerifyPassword(String value, String passhash)

        [Fact]
        public void VerifyPassword_OnNullValueFails()
        {
            Assert.False(crypter.VerifyPassword(null, ""));
        }

        [Fact]
        public void VerifyPassword_OnNullHashFails()
        {
            Assert.False(crypter.VerifyPassword("", null));
        }

        [Fact]
        public void VerifyPassword_VerifiesPasshash()
        {
            Assert.True(crypter.VerifyPassword("Test", "$2a$13$g7QgmyFicKkyI4kiHM8XQ.LfBdpdcLUbw1tkr9.owCN5qY9eCj8Lq"));
        }

        #endregion
    }
}
