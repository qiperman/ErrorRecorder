using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models.ViewModels;
using TestTask.Models;
using Microsoft.AspNetCore.Authorization;

namespace TestTask.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        private IErrorRepository repository;    //Errors repository
        private IUserRepository userRepository; //Users repository

        public int PageSize = 5; //Number of errors per page


        public ErrorController(IErrorRepository repository, IUserRepository userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
        }

        //The main method returns a list of sorted errors 
        public ViewResult List(int page = 1, SortState sortOrder = SortState.DateDesc)
        {
            IEnumerable<Error> errors = repository.Errors;                    
            ViewData["DateSort"] = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["StatusSort"] = sortOrder == SortState.StatusAsc ? SortState.StatusDesc : SortState.StatusAsc;
            ViewData["UrgencySort"] = sortOrder == SortState.UrgencyAsc ? SortState.UrgencyDesc : SortState.UrgencyAsc;
            ViewData["CriticalitySort"] = sortOrder == SortState.CriticalityAsc ? SortState.CriticalityDesc : SortState.CriticalityAsc;

            //Sort errors by sortOrder
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    errors = errors.OrderByDescending(s => s.User.Name).ThenBy(s=>s.User.SName);
                    break;
                case SortState.NameAsc:
                    errors = errors.OrderBy(s => s.User.Name);
                    break;
                case SortState.DateAsc:
                    errors = errors.OrderBy(s => s.Date);
                    break;
                case SortState.StatusAsc:
                    errors = errors.OrderBy(s => s.status);
                    break;
                case SortState.StatusDesc:
                    errors = errors.OrderByDescending(s => s.status);
                    break;

                case SortState.UrgencyAsc:
                    errors = errors.OrderBy(s => s.urgency);
                    break;
                case SortState.UrgencyDesc:
                    errors = errors.OrderByDescending(s => s.urgency);
                    break;

                case SortState.CriticalityAsc:
                    errors = errors.OrderBy(s => s.criticality);
                    break;
                case SortState.CriticalityDesc:
                    errors = errors.OrderByDescending(s => s.criticality);
                    break;
                default:
                    errors = errors.OrderByDescending(s => s.Date);
                    break;
            }

            //Return View sorted by sortOrder equal to the number of errors per page
            return View(new ErrorListViewModel
            {
                Errors = errors
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                Paginginfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Errors.Count()
                },
                SortState = sortOrder
                
            });
        }

        //Edit Error by Id
        public ViewResult Edit(int errorId)
        {
            Error error = repository.Errors.FirstOrDefault(e => e.ErrorId == errorId);
            if (error.status == Status.Closed)
            {
                return View("View", error);
            }
            return View(new EditViewModel { Error = error });
        }

        //Get method ading error
        public ViewResult Add() => View();

        //Post method ading error
        [HttpPost]
        public IActionResult Add(Error error)
        {
            if (ModelState.IsValid)
            {
                Error newError = new Error();
                newError.Desciption = error.Desciption;
                newError.SmallDescription = error.SmallDescription;
                newError.urgency = error.urgency;
                newError.criticality = error.criticality;
                newError.User = userRepository.Users.FirstOrDefault(u => u.Login == User.Identity.Name);

                HistoryAction newAction = new HistoryAction
                {
                    user = newError.User,
                    historyAct = HistoryAct.Create
                };

                newError.History.Add(newAction);


                repository.SaveError(newError);
            }
            return RedirectToAction("List");
        }

        //Post method of changing status of error
        [HttpPost]
        public IActionResult ChangeStatus(EditViewModel errorModel)
        {
            if (ModelState.IsValid)
            {
                Error error = repository.Errors.FirstOrDefault(e => e.ErrorId == errorModel.Error.ErrorId);
                if (error.status != Status.Closed)
                {
                    HistoryAct historyAct = HistoryAct.Resolve;

                    switch (errorModel.addHistory.Activity)
                    {
                        case Activity.Open:
                            {
                                historyAct = HistoryAct.Open;
                                error.status = Status.Open;
                                break;
                            }
                        case Activity.Resolve:
                            {
                                historyAct = HistoryAct.Resolve;
                                error.status = Status.Resovled;
                                break;
                            }
                        case Activity.Close:
                            {
                                historyAct = HistoryAct.Close;
                                error.status = Status.Closed;
                                break;
                            }
                    }
                    HistoryAction newHistory = new HistoryAction
                    {
                        user = userRepository.Users.FirstOrDefault(u => u.Login == User.Identity.Name),
                        Comment = errorModel.addHistory.Comment,
                        historyAct = historyAct
                    };
                    error.History.Add(newHistory);
                    repository.SaveError(error);
                }

            }
            return RedirectToAction("Edit", new { errorId = errorModel.Error.ErrorId });
        }
        //Post method of edit error 
        [HttpPost]
        public IActionResult Edit(EditViewModel errorModel)
        {
            if (ModelState.IsValid)
            {
                repository.SaveError(errorModel.Error);
                return RedirectToAction("List");
            }
            return RedirectToAction("Add");
        }


        public ViewResult ViewError(int errorId) => View("View", repository.Errors.FirstOrDefault(e => e.ErrorId == errorId));

    }
}

public enum SortState
{
    NameAsc,
    NameDesc,
    DateAsc,
    DateDesc,
    StatusAsc,
    StatusDesc,
    UrgencyAsc,
    UrgencyDesc,
    CriticalityAsc,
    CriticalityDesc,
}
