﻿namespace Naxxum.WeeCare.Authentification.Domain.Entities;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(int id) : base(id)
    {
    }
}