// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using Moq;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Application.IntegrationTests
{
    public class TestCommon
    {

         public static  IHttpContextAccessor SetAndGetAuthorisedTestContext(TestServer server){

            var accesor = server.Services
                .GetService<IHttpContextAccessor>();

            var test_context = TestCommon.GetTestHttpContext(
                TestCommon.GetDefaultTestUserClaims()
            );

            accesor.HttpContext = test_context;

            return accesor;
        }

        public static IHttpContextAccessor SetAndGetUnAuthorisedTestConetxt(TestServer server){

            var accesor = server.Services
                .GetService<IHttpContextAccessor>();

            var test_context = TestCommon.GetTestHttpContext();

            accesor.HttpContext = test_context;

            return accesor;
        }

        public static HttpContext GetTestHttpContext(List<Claim> claims = null){

            if(claims == null){
                return new DefaultHttpContext();
            }else{

                var mocked_idenity = new Mock<IIdentity>();

                mocked_idenity.Setup(e=>e.IsAuthenticated).Returns(true);

                mocked_idenity.Setup(e=>e.Name)
                    .Returns(claims.Where(e=>e.Type == ClaimTypes.Name ).First().Value);

                mocked_idenity.Setup(e=>e.AuthenticationType).Returns("SomeType");

                var claimsIdentity = new ClaimsIdentity(mocked_idenity.Object,claims);

                var principal = new ClaimsPrincipal(claimsIdentity);

                var default_ctx = new DefaultHttpContext();

                default_ctx.User = principal;

                return default_ctx;
            }

        }

        public static List<Claim> GetDefaultTestUserClaims(){
            return  new(){
                new Claim(ClaimTypes.Name,"Zlobor"),
                new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
            };
        }
        
    }
}