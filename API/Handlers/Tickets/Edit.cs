using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Images;
using API.Infrastructure.Security;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Tickets
{
    public class Edit
    {
        public class Command : IRequest
        {
            public int post_id { get; set; }

            public string description { get; set; }

            public string title { get; set; }

            public int product_id { get; set; }

            public int status_id { get; set; }

            //If attachement
            public IFormFile image { get; set; }

            public bool deletingImage { get; set; }

        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            private readonly PhotoAccessor photoAccessor;
            private readonly UserManager<User> userManager;
            private readonly UserAccessor userAccessor;
            private readonly RoleManager<Role> roleManager;
            public Handler(ApplicationDBContext context, PhotoAccessor photoAccessor, UserManager<User> userManager, RoleManager<Role> roleManager, UserAccessor userAccessor)
            {
                this.roleManager = roleManager;
                this.userAccessor = userAccessor;
                this.userManager = userManager;
                this.photoAccessor = photoAccessor;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Get current user
                var current_user = await userManager.FindByEmailAsync(userAccessor.getCurrentUsername());

                var current_user_role_list = await userManager.GetRolesAsync(current_user);

                Role current_user_role = null;

                if (current_user_role_list.Count > 0)
                {
                    var current_user_role_string = current_user_role_list[0];
                    current_user_role = await roleManager.FindByNameAsync(current_user_role_string);
                }

                Ticket ticket = await context.tickets.FindAsync(request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                if (!(current_user.Id == ticket.author_id || (current_user_role != null && current_user_role.can_moderate))) throw new RestException(HttpStatusCode.Forbidden, new { user = "You don't have the permission to do this" });

                ticket.description = request.description ?? ticket.description;
                ticket.title = request.title ?? ticket.title;

                if (request.product_id != 0)
                    ticket.product_id = request.product_id;

                if (request.status_id != 0)
                    ticket.status_id = request.status_id;

                //Handling attachments

                //Find current attachment
                var current_attachement = await context.attachments.FindAsync(ticket.attachment_id);

                //Initialize new attachemnt
                Attachment new_attachment = null;

                //If the request contains an attachment, and the current ticket has no attachement...
                //Then we have a ticket, initially without an attachement, and the user wants to add one to it...
                //So upload the requested attachement, and assign to ticket
                if (request.image != null && current_attachement == null)
                {
                    var uploadAttachmentResult = photoAccessor.AddPhoto(request.image);

                    new_attachment = new Attachment
                    {
                        Id = uploadAttachmentResult.PublicId,
                        url = uploadAttachmentResult.Url
                    };

                    //Set the ticket.attachment to new_attachment (this may null)
                    ticket.attachment = new_attachment;
                }

                //If the ticket initially has an attachement, and the user has selected to delete the attachement...
                //Delete the attachement from the ticket
                if (current_attachement != null && request.deletingImage)
                {
                    photoAccessor.DeletePhoto(current_attachement.Id);
                    context.attachments.Remove(current_attachement);

                    //If the user has also selected to add a new attachement instead of the old one...
                    //Upload this new attachement, and apply it to the ticket
                    if (request.image != null)
                    {
                        var uploadAttachmentResult = photoAccessor.AddPhoto(request.image);

                        new_attachment = new Attachment
                        {
                            Id = uploadAttachmentResult.PublicId,
                            url = uploadAttachmentResult.Url
                        };

                        //Set the ticket.attachment to new_attachment (this may null)
                        ticket.attachment = new_attachment;
                    }
                }

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}