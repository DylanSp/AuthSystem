using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Controllers
{
    public struct TestStruct
    {
        public int TestMember { get; }
        
        public TestStruct(int testMember)
        {
            TestMember = testMember;
        }
    }

    public struct TestStructWithSetter
    {
        public int TestMember { get; set; }
    }

    [Route("api/valuetesting")]
    [ApiController]
    public class ValueTypeTestingController : ControllerBase
    {
        [HttpGet]
        [Route("resources/{value}")]
        public ActionResult<string> TestResourceValue([FromRoute] ResourceValue value)
        {
            return "ok";
        }

        [HttpGet]
        [Route("users/{userId:guid}")]
        public ActionResult<string> TestUserId([FromRoute] UserId userId)
        {
            return "ok";
        }

        [HttpPost]
        [Route("simpleSetter")]
        public ActionResult<string> TestSimpleDeserializationThatShouldWork([FromBody] TestStructWithSetter testStruct)
        {
            // given { "TestMember": 3 }
            // testStruct.TestMember should equal 3, actually is 3
            return "ok";
        }

        [HttpPost]
        [Route("simple")]
        public ActionResult<string> TestSimpleDeserialization([FromBody] TestStruct testStruct)
        {
            // given { "TestMember": 3 }
            // testStruct.TestMember should equal 3, actually is 0
            return "ok";
        }

        [HttpPost]
        [Route("hashes")]
        public ActionResult<string> TestDeserialization([FromBody] HashedPassword hashedPassword)
        {
            // given { "Base64PasswordHash": "hash", "Base64Salt": "salt" }
            // should set values, actually gives validation error
            return "ok";
        }
    }
}