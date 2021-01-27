using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreditOne.Microservices.BuildingBlocks.MaskEncryptSensitiveData.Test
{
    [TestClass]
    public class MaskEncryptSensitiveDataTest
    {
        #region No sensitive data

        [TestMethod]
        public void MsgWithTextNoSensitiveDataReturnsMsg()
        {
            //Arrange 
            string message = "Text Text Text";
            string expectMessage = "Text Text Text";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        #endregion

        #region Credit Card Test

        [TestMethod]
        public void MsgWithCreditCardWithSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "4111 1111 1111 1111";
            string expectMessage = "XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithCreditCardWithSlashReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "4111-1111-1111-1111";
            string expectMessage = "XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithCreditCardWithNoSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "4111111111111111";
            string expectMessage = "XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithTextCreditCardWithSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "Text 4111 1111 1111 1111";
            string expectMessage = "Text XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithCreditCardWithSlashTextReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "4111-1111-1111-1111 Text";
            string expectMessage = "XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=) Text";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithTextCreditCardWithNoSpacesTextReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "Text 5166972301705017 Text";
            string expectMessage = "Text XXXX-XXXX-XXXX-5017 (PidoOiHjfZ5aObJZskBlfrtN26wGRothBAeaLkC4oGY=) Text";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        #endregion

        #region SSN Test

        [TestMethod]
        public void MsgWithSSNWithSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange
            string message = "111 11 1111";
            string expectMessage = "XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithSSNWithSlashReturnsMsgEncryptedAndMasked()
        {
            //Arrange
            string message = "111-11-1111";
            string expectMessage = "XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithSSNWithNoSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange
            string message = "111111111";
            string expectMessage = "XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithTextSSNWithSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange
            string message = "Text 111 11 1111";
            string expectMessage = "Text XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithSSNWithSlashTextReturnsMsgEncryptedAndMasked()
        {
            //Arrange
            string message = "111-11-1111 Text";
            string expectMessage = "XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==) Text";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithTextSSNWithNoSpacesTextReturnsMsgEncryptedAndMasked()
        {
            //Arrange
            string message = "Text 111111111 Text";
            string expectMessage = "Text XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==) Text";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        #endregion

        #region Credit Card and SSN Test

        [TestMethod]
        public void MsgWithCreditCardWithSpacesSSNWithSlashReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "4111 1111 1111 1111111-11-1111";
            string expectMessage = "XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=)XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithCreditCardWithSlashSSNNoSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "4111-1111-1111-1111111111111";
            string expectMessage = "XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=)XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithSSNWithSpacesTextCreditCardWithNoSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange 
            string message = "111 11 11114111111111111111";
            string expectMessage = "XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithTextCreditCardWithSpacesTextSSNWithNoSpacesReturnsMsgEncryptedAndMasked()
        {
            //Arrange  
            string message = "Text 4111 1111 1111 1111Text111111111";
            string expectMessage = "Text XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=)TextXXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithSSNWithSlashCreditCardWithSlashTextReturnsMsgEncryptedAndMasked()
        {
            //Arrange  
            string message = "111-11-11114111-1111-1111-1111 Text";
            string expectMessage = "XXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)XXXX-XXXX-XXXX-1111 (+XDw4dHffbtrPRFaqSZOkcvy32hExbKIFE2b3dEaA7s=) Text";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        [TestMethod]
        public void MsgWithTextCreditCardWithNoSpacesTextSSNWithSpacesTextReturnsMsgEncryptedAndMasked()
        {
            //Arrange  
            string message = "Text5166972301705017Text111 11 1111Text";
            string expectMessage = "TextXXXX-XXXX-XXXX-5017 (PidoOiHjfZ5aObJZskBlfrtN26wGRothBAeaLkC4oGY=)TextXXX-XX-1111 (B4J7P3KFPEm2z7IQbsm/Zw==)Text";

            //Act
            var resultMsg = message.MaskAndEncryptSensitiveData();

            //Assert
            Assert.AreEqual<string>(expectMessage, resultMsg);
        }

        #endregion
    }
}
