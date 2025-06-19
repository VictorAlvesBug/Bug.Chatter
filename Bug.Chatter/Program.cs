var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*
##### Use Cases #####

--> User
User GetUser(string id)
User RegisterUser(string name, string phoneNumber)
User UpdateUserName(string id, string name)
User UpdateUserBiography(string id, string biography)


--> Chat
//GetChat(string id)


--> Message


--> User & Chat
Chat CreateDirectChat(string[] userIds)
Chat CreateGroupChat(string name, string[] userIds)
Chat[] ListUserChats(string userId)
User[] ListChatUsers(string chatId)
Chat AddUserToChat(string chatId, string userId)
void RemoveUserFromChat(string chatId, string userId)
void DeleteChat(string chatId)


--> Chat & Message
Message[] ListMessages(string chatId);


--> User & Chat & Message
Message SendMessage(string chatId, string userId, string content)


--> User & Message
Message UpdateMessageVisualizationStatusForUser(string messageId, string userId, MessageVisualizationStatus status)
Message DeleteMessageForUser(string messageId, string userId)


##### Entities #####

--> User
string Id
string Name
string PhoneNumber

--> Chat
string Id
ChatType ChatType
string Name

--> Message
string Id
string content
MessageStatus status

--> ChatType
DirectChat
GroupChat

--> MessageStatus
Typing
Sending
Sent
Deleting
Deleted


--> MessageVisualizationStatus
Delivered
Read

*/