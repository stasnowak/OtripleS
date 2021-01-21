﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherAttachments.Exceptions
{
    public class LockedTeacherAttachmentException : Exception
    {
        public LockedTeacherAttachmentException(Exception innerException)
            : base("Locked TeacherAttachment record exception, please try again later.", innerException)
        { }
    }
}
