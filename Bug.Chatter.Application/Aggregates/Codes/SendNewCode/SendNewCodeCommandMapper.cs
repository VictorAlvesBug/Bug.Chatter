using Bug.Chatter.Application.Common;
using Bug.Chatter.Domain.Aggregates.Codes;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Application.Aggregates.Codes.SendNewCode
{
	internal class SendNewCodeCommandMapper : ICommandMapper<SendNewCodeCommand, Code>
	{
		public Code Map(SendNewCodeCommand input)
		{
			var phoneNumber = PhoneNumber.Create(input.PhoneNumber);
			return Code.CreateNew(phoneNumber);
		}
	}
}
