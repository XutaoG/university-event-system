using CollegeEvent.API.Models;

namespace CollegeEvent.API.Repositories;

public interface IEventRepository
{
	Task<PublicEvent?> CreatePublicEvent(PublicEvent publicEvent);

	Task<PrivateEvent?> CreatePrivateEvent(PrivateEvent privateEvent);

	Task<RSOEvent?> CreateRsoEvent(RSOEvent rsoEvent);

	Task<PublicEvent?> GetPublicEventById(int id);

	Task<PrivateEvent?> GetPrivateEventById(int id);

	Task<RSOEvent?> GetRsoEventById(int id);

	Task<Event?> GetEventById(int id);

	Task<Event?> UpdateEvent(int id, Event event_);

	Task<Event?> DeleteEvent(int id);

	Task<PublicEvent?> SetPublicEventApproved(int id);
}