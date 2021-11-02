/* Models */

namespace AdvancedSoftwareDesign
{
  public class EntityValue<T> { T Value; }

  public class TodoStatusId : EntityValue<Guid> { }
  public class TodoStatus { TodoStatusId id; string name; }

  public class TodoPriorityId : EntityValue<Guid> { }
  public class TodoPriority { TodoPriorityId id; string name; short order; }

  public class TodoPrioritySetId : EntityValue<Guid> { }
  public class TodoPrioritySet { TodoPrioritySetId id; List<TodoPriority> priorities; }

  public class StickerId : EntityValue<Guid> { }
  public class Sticker { StickerId id; Uri location; }

  public class DueDate : EntityValue<DateTime> { }

  public enum TodoVisibility { PUBLIC, PRIVATE }

  public class TodoId : EntityValue<Guid> { }
  public class Todo { TodoId id; string name; TodoStatus status; TodoPriority priority; DueDate dueDate; List<Sticker> stickers; TodoVisibility visibility; }

  public class TodoListId : EntityValue<Guid> { }
  public class TodoList { TodoListId id; string name; List<Todo> items; TodoPrioritySetId? prioritySetId; }

  public class UserId : EntityValue<Guid> { }
  public class User
  {
    UserId id;
    string name;

    /* We support the user to have multiple todo lists although for now the UI doesn't support it. */
    List<TodoListId> todoLists;
  }

  public class Friendship { UserId user1; UserId user2; }
  public class FriendTodoListAccess { UserId owner; UserId friend; TodoListId todoListId; }

  public enum ActivityOperation { CREATE, UPDATE, DELETE }
  public class EntityData : EntityValue<string> { }
  public class ActivityLog { UserId userId; ActivityOperation operation; DateTime date; EntityData previousData; EntityData newData; }

  /* Services that will support the model operations */

  public class CreateTodoListDto { UserId owner; string todoListName; }

  public class GetTodosDto { UserId userId; TodoStatus? todoStatus; TodoPriority? todoPriority; }
  public class GetFriendTodosDto { UserId friendRequesting; TodoListId todoList; GetTodosDto filter; }

  public interface ITodoListService
  {
    TodoList Create(CreateTodoListDto todoListDto);
    void Delete(TodoListId todoListId);
    List<Todo> GetTodos(GetTodosDto filter);
    void AddToDo(TodoListId todoListId, TodoId todoId);
    void RemoveToDo(TodoListId todoListId, TodoId todoId);
    List<Todo> GetFriendsTodoList(GetFriendTodosDto dto);
  }

  public class ChangeTodoStatusDto { User userId; TodoId todoId; TodoStatusId status; }
  public class SetTodoVisibilityDto { UserId userId; TodoId todoId; TodoVisibility visibility; }
  public class CreateTodoDto { UserId userId; TodoListId todoListId; string todoName; }

  public interface ITodoService
  {
    TodoId Create(CreateTodoDto dto);
    void UpdateState(ChangeTodoStatusDto dto);
    void UpdateVisibility(SetTodoVisibilityDto dto);
  }

  public class CreateUserDto { string name; }
  public interface IUserService
  {
    UserId Create(CreateUserDto dto);
  }

  public class ManageFriendshipDto { UserId user1; UserId user2; }
  public interface IFriendshipService
  {
    void AddFriend(ManageFriendshipDto dto);
    void RemoveFriend(ManageFriendshipDto dto);
  }

  public class ManageFriendAccessToTodoListDto { UserId owner; UserId friend; TodoListId todoListId; }
  public interface IFriendTodoListAccess
  {
    void GiveAccess(ManageFriendAccessToTodoListDto dto);
    void RemoveAccess(ManageFriendAccessToTodoListDto dto);
  }

  public class GetTodoListHistoryDto { UserId userId; DateTime todoListHistoryDate; }
  public class TodoListHistoryData { DateTime todoListHistoryDate; TodoList todoList; }
  public interface ITodoListHistoryService
  {
    // Uses the Activity Log to recreate the TodoList at a certain point in time
    TodoListHistoryData Get(GetTodoListHistoryDto dto);
  }
}