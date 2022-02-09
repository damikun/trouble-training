using System;
using MediatR;
using SharedCore.Aplication.Services;

namespace SharedCore.Aplication.Interfaces
{

    public interface IScheduler
    {
        string Enqueue(
            IRequest request,
            string description = null);

        void Schedule(
            IRequest request,
            DateTimeOffset scheduleAt,
            string description = null);

        void Schedule(
            IRequest request,
            TimeSpan delay,
            string description = null);

        void Schedule(
            MediatorSerializedObject mediatorSerializedObject,
            TimeSpan delay,
            string description = null);

        void ScheduleRecurring(
            IRequest request,
            string name,
            string cronExpression,
            string description = null,
            string queue = "default");

        void ScheduleRecurring(
            MediatorSerializedObject mediatorSerializedObject,
            string name,
            string cronExpression,
            string description = null,
            string queue = "default");
    }
}