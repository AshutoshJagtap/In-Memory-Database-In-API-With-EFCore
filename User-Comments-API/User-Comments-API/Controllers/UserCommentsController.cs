using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User_Comments_API.DatabaseContext;
using User_Comments_API.DbModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User_Comments_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCommentsController : ControllerBase
    {
        private readonly ApiContext _apiContext;

        public UserCommentsController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        // GET: api/<UserCommentsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var allData = await _apiContext.Users.Include(x => x.Posts).ToListAsync();
            if (allData == null || allData.Count() == 0)
            {
                return NotFound();
            }

            var response = allData.Select(x => new
            {
                id = x.Id,
                firstName = x.FirstName,
                lastName = x.LastName,
                posts = x.Posts.Select(p => new
                {
                    id = p.Id,
                    UserId = x.Id,
                    content = p.Content
                })
            });

            return Ok(response);

        }

        // GET api/<UserCommentsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(Guid id)
        {
            var allData = await _apiContext.Users.Include(x => x.Posts).Where(x => x.Id == id).ToArrayAsync();
            if (allData == null || allData.Count() == 0)
            {
                return NotFound();
            }
            var response = allData.Select(x => new
            {
                id = x.Id,
                firstName = x.FirstName,
                lastName = x.LastName,
                posts = x.Posts.Select(p => new
                {
                    id = p.Id,
                    UserId = x.Id,
                    content = p.Content
                })
            });

            return Ok(response);
        }

        // POST api/<UserCommentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            foreach (Post item in user.Posts)
            {
                item.Id = Guid.NewGuid();
                item.UserId = user.Id;
            }
            await _apiContext.Users.AddAsync(user);
            await _apiContext.SaveChangesAsync();

            var response = new
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                posts = user.Posts.Select(p => new
                {
                    id = p.Id,
                    content = p.Content
                })
            };
            return Created($"api/UserComments/{user.Id}", response);

        }

        // PUT api/<UserCommentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _apiContext.Users.Update(user);
            //_apiContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _apiContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var response = new
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                posts = user.Posts.Select(p => new
                {
                    id = p.Id,
                    content = p.Content
                })
            };
            return Ok(response);
        }

        // DELETE api/<UserCommentsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(Guid id)
        {
            var user = _apiContext.Users.Include(x => x.Posts).FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _apiContext.Users.Remove(user);
            await _apiContext.SaveChangesAsync();
            var response = new
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                posts = user.Posts.Select(p => new
                {
                    id = p.Id,
                    content = p.Content
                })
            };
            return Ok(response);
        }

        private bool UserExists(Guid id)
        {
            return _apiContext.Users.Any(x => x.Id == id);
        }
    }
}
