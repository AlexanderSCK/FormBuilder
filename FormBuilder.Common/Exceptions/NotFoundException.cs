﻿using System.Net;

namespace FormBuilder.Common.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(Guid id) : base($"Form with id: {id} not found", HttpStatusCode.NotFound) { }
}