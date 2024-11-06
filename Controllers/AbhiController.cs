using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebApiEx1.Context;
using WebApiEx1.DTO;
using WebApiEx1.Models;

namespace WebApiEx1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AbhiController : ControllerBase
    {
        //private static  List<ToDoItem> _toDoItems = new List<ToDoItem>
        //{
        //    new ToDoItem{Id=1,Title="WakeUp",IsCompleted=true},
        //    new ToDoItem{Id=2,Title="HadBreakfast",IsCompleted=true},
        //    new ToDoItem{Id=3,Title="WentToOffice",IsCompleted=false},

        //};
        private readonly ToDoContext _toDoContext;

        public AbhiController(ToDoContext toDoContext)
        {
            _toDoContext = toDoContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        { 
           // if (_toDoItems.Count == 0)
           // {
           //     return BadRequest();
           // }
           //var todoitemdtos = _toDoItems.Select(item => MapToDoItemToDTO(item)).ToList();
            var _toDoItem = _toDoContext.ToDoItems.ToList();

            return Ok(_toDoItem);
        }

        [HttpGet("{id}")]
        public IActionResult GetToDoItemById(int id)
        {

            var _toDoItem = _toDoContext.ToDoItems.FirstOrDefault(item => item.Id == id);
            if (_toDoItem == null)
            {
                return BadRequest();
            }
           // var _toDoItemDto = MapToDoItemToDTO(_toDoItem);
            //  var _toDoItem = _toDoContext.ToDoItems.ToList();

            return Ok(_toDoItem);
        }

        ///<summarize>
        ///For Create ToDoItem in the List
        ///</summarize>
        [HttpPost]
        public IActionResult CreateToDoItem([FromBody]ToDoItem newitemdto)
        {
            if(newitemdto==null)
            { return BadRequest(); }
            var newitem = new ToDoItem
            {
                Title = newitemdto.Title,
                IsCompleted = newitemdto.IsCompleted
            };
            //Generate new id for the newly created item
            //int newid=_toDoContext.ToDoItems.Max(item=>item.Id)+1;
            //newitem.Id = newid;
            _toDoContext.Add(newitem);
            _toDoContext.SaveChanges();

            //return newly created to-do item along with 201 created status code
            return CreatedAtAction(nameof(GetToDoItemById), new { id = newitem.Id }, newitem);
        
        }

        ///<summary>
        ///Used For Updating The ToDoItem
        ///</summary>
        ///<param name="id"></param>
        

        [HttpPut("{id}")]
        public IActionResult UpdateToDoItemById(int id, [FromBody]ToDoItem updatedContext)
        {
            if (updatedContext == null)
            {
                return BadRequest();
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingitem= _toDoContext.ToDoItems.FirstOrDefault(item=>item.Id==id);
            existingitem.Title = updatedContext.Title;
            existingitem.IsCompleted= updatedContext.IsCompleted;
             var entity=_toDoContext.ToDoItems.Update(existingitem);
            _toDoContext.SaveChanges();
            return Ok(entity.Entity);

            //if(existingitem==null)
            //{
            //    return NotFound();
            //}
            // existingitem.Title= updatedDTO.Title;
            //existingitem.IsCompleted= updatedDTO.HasCompleted;
            //var UpdateditemDTO = MapToDoItemToDTO(existingitem);



        }

        ///<summary>
        ///Updating The Partial Content of ToDoItem 
        ///</summary>
        
        [HttpPatch("{id}")]
        public IActionResult UpdateToDoItemByIdUsingPatch(int id, [FromBody]JsonPatchDocument<ToDoItem> patchDocument)
        {
            if(patchDocument==null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Existingitem= _toDoContext.ToDoItems.FirstOrDefault(item=> item.Id==id);
            //Applying Changes Using Patch
            patchDocument.ApplyTo(Existingitem);

            return Ok(patchDocument);
        }
        ///<summary>
        ///Delete The ToDoItem by Id
        ///</summary>

        [HttpDelete("{id}")]
        public IActionResult DeleteToDoItem(int id)
        {
            var existingitem = _toDoContext.ToDoItems.FirstOrDefault(item => item.Id == id);
            if (existingitem == null)
            {
                return NotFound();
            }
            _toDoContext.Remove(existingitem);
            _toDoContext.SaveChanges();

            return Ok(existingitem);
        }
       

        /// <summarize>
        ///These Method Maps ToDoItem Model To ToDoItemDto
        /// </summarize>
        /// <param name="item"></param>

        private ToDoItemDTO MapToDoItemToDTO(ToDoItem item)
        {
           
                return new ToDoItemDTO
                {
                    Id = item.Id,
                    Title = item.Title,
                    HasCompleted = item.IsCompleted

                };
         }
       
     
        }
    }
