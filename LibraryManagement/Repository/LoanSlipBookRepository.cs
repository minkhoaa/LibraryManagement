﻿using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class LoanSlipBookRepository : ILoanSlipBookRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly IAuthenRepository _account;


        public LoanSlipBookRepository(LibraryManagermentContext context, IAuthenRepository authen)
        {
            _account = authen;
            _context = context;
        }

        public Task<ApiResponse<LoanSlipBookResponse>> addLoanSlipBookAsync(LoanSlipBookRequest request)
        {
            //var newLoanSlipBook = new LoanSlipBook
            //{
            //    IdReader = request.IdReader,
            //    IdTheBook = request.IdTheBook,
            //    BorrowDate = DateTime.SpecifyKind(request.BorrowDate, DateTimeKind.Utc),
            //    ReturnDate = DateTime.SpecifyKind(request.ReturnDate, DateTimeKind.Utc)
            //};
            //var thebook = _context.TheBooks.FirstOrDefaultAsync(tb => tb.)
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> deleteLoanSlipBookAsync(Guid idLoanSlipBook)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LoanSlipBookResponse>> getListLoanSlipBook(string token)
        {
            var user = await _account.AuthenticationAsync(token);
            if (user == null) return null;
            var result = await _context.LoanSlipBooks.Select(a => new LoanSlipBookResponse
            {
                IdLoanSlipBook = a.IdLoanSlipBook,
                IdTheBook = a.IdTheBook,
                IdReader = a.IdReader,
                BorrowDate = a.BorrowDate,
                ReturnDate = a.ReturnDate,
                FineAmount = a.FineAmount
            }).ToListAsync();
            return result; 
        }

        public Task<ApiResponse<LoanSlipBookResponse>> updateLoanSlipBookAsync(LoanSlipBookRequest request, Guid idLoanSlipBook)
        {
            throw new NotImplementedException();
        }
    }
}
